using System.ComponentModel.DataAnnotations;

namespace BlogsAPI.Dtos
{
    public class LoginDto
    { 

        private string _email;
        private string _password;

        [MinLength(7)]
        [MaxLength(254)]
        [EmailAddress]
        public string Email
        {
            get => _email;
            set => _email = SantizeData.SanitizeString(value);
        }

        public string Password
        {
            get => _password;
            set => _password = SantizeData.SanitizeString(value);
        }
    }
}
