using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;

namespace DAPManSWebReports.Domain.ErrorReportService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly string _receiverEmail;

        public EmailService(IOptions<SmtpSettings> smtpSettings, string receiverEmail)
        {
            _smtpSettings = smtpSettings.Value;
            _receiverEmail = receiverEmail;
        }

        public async Task SendErrorReportAsync(ReportError reportError)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("shipelov denis", _receiverEmail));
                message.Subject = "Error Report";
                var builder = new BodyBuilder
                {
                    TextBody = $@"Description: {reportError.description}
                  Email: {reportError.email}
                  URL: {reportError.url}"
                };

                if (reportError.file != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await reportError.file.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        var mimeType = GetContentType(reportError.file.FileName);
                        var attachment = new MimePart(mimeType)
                        {
                            Content = new MimeContent(memoryStream, ContentEncoding.Default),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = reportError.file.FileName
                        };
                        builder.Attachments.Add(attachment);
                    }
                }
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
                        await client.AuthenticateAsync(_smtpSettings.SenderEmail, _smtpSettings.Password);
                        await client.SendAsync(message);

                        await client.DisconnectAsync(true);

                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                    }
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't send email: {ex.Message}", ex);
                throw new InvalidOperationException($"Couldn't send email: {ex.Message}", ex);
            }
        }
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".html" => "text/html",
                ".xml" => "application/xml",
                _ => "application/octet-stream",
            };
        }
    }
}
