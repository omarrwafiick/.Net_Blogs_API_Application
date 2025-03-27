namespace BlogsAPI.Interfaces
{
    public interface IEmailSender 
    {
        Task SendMailAsync(string mailto, string subject, string body);
    }
}
