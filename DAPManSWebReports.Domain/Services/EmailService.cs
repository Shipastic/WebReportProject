using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;

using Microsoft.Extensions.Options;

using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace DAPManSWebReports.Domain.Services
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
                using var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                    EnableSsl = true,
                    Host = _smtpSettings.Server
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = "Error Report",
                    Body = $@"Description: {reportError.description}
                              Email: {reportError.email}
                              URL: {reportError.url}"
                };

                mailMessage.To.Add(_receiverEmail);

                if (reportError.file != null)
                {
                    var memoryStream = new MemoryStream();
                    await reportError.file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var attachment = new Attachment(memoryStream, reportError.file.FileName);
                    mailMessage.Attachments.Add(attachment);
                }
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't send email: {ex.Message}", ex);
                throw new InvalidOperationException($"Couldn't send email: {ex.Message}", ex);
            }
        }
    }
}
