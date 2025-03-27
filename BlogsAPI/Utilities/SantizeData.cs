using System.Text.RegularExpressions;

namespace BlogsAPI.Utilities
{
    public static class SantizeData
    {
        private readonly static HtmlSanitizer sanitizer = new HtmlSanitizer();
        public static string SanitizeString(string str)
        {
            var sanitized = sanitizer.Sanitize(str);
            return Regex.Replace(sanitized, @"<[^>]+>\s*</[^>]+>", string.Empty);
        }
    }
}
