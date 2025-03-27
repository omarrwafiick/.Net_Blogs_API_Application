using BlogsAPI.Models;

namespace BlogsAPI.Extensions
{
    public static class AppSettingsExtensions
    {
        public static void ConfigAppSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
        }
    }
}
