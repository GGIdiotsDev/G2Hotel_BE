using Microsoft.Extensions.Options;
using g2hotel_server.Helper;
using g2hotel_server.Services.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using g2hotel_server.DTOs;

namespace g2hotel_server.Services.Implements
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public void Send(MailDTO mail)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(mail.From ?? _mailSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(mail.To));
            email.Subject = mail.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = mail.Html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}