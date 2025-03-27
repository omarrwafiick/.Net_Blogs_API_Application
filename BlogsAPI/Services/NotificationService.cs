namespace BlogsAPI.Services
{
    public class NotificationService : INotificationService<AppUser>
    {
        private readonly IMailingService _mailing;
        public NotificationService(IMailingService mailing)
        {
            _mailing = mailing;
        }
        public async void Notification(List<AppUser> users, string message)
        {
            foreach (var user in users)
                await _mailing.SendMail(user.Email, "Notification", $"We notify you that {message}", " ");
        }
    }
}
