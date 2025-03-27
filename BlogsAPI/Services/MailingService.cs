
namespace BlogsAPI.Services
{
    public class MailingService : IMailingService
    {
        private readonly IEmailSender _mailing;
        public MailingService(IEmailSender mailing)
        {
            _mailing = mailing;
        }
        public async Task SendMail(string email, string subject, string body, string data)
        {
            await _mailing.SendMailAsync(email, subject, body + data);
        }
    }
}
