using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = "Error Report",
                    Body = $@"Description: {reportError.Description}
                              Email: {reportError.Email}
                              URL: {reportError.Url}"
                };

                mailMessage.To.Add(_receiverEmail);

                if (reportError.File != null)
                {
                    var memoryStream = new MemoryStream();
                    await reportError.File.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var attachment = new Attachment(memoryStream, reportError.File.FileName);
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
