namespace BlogsAPI.Interfaces
{
    public interface IMailingService 
    {
        Task SendMail(string email, string subject, string body, string data);
    }
}
