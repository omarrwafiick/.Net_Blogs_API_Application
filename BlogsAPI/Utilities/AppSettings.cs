namespace BlogsAPI.Models
{
    public class AppSettings
    {
        public string JWTSecret { get; set; }
        public string JWTIssuer { get; set; }
        public string JWTAudience { get; set; }
    }
}
