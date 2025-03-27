 
namespace BlogsAPI.Interfaces
{
    public interface INotificationService<T> : IScopedService<T> where T : class
    {
        void Notification(List<AppUser> users, string message);
    }
}
