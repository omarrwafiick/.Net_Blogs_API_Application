using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BlogsAPI.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _mailsettings;
        public EmailSender(IOptions<EmailSettings> mail)
        {
            _mailsettings = mail.Value;
        }
        public async Task SendMailAsync(string mailto, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailsettings.Email),
                Subject = subject,
            };
            email.To.Add(MailboxAddress.Parse(mailto));
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailsettings.DisplayName, _mailsettings.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(_mailsettings.Host, _mailsettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailsettings.Email, _mailsettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
