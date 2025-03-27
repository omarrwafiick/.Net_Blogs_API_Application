using System.ComponentModel.DataAnnotations; 

namespace BlogsAPI.Dtos
{
    public class ContactDto
    {  
        private string _message;
        private string _email;

        [MaxLength(350)]
        [MinLength(10)]
        public string Message
        {
            get => _message;
            set => _message = SantizeData.SanitizeString(value);
        }

        [MinLength(7)]
        [MaxLength(254)]
        [EmailAddress]
        public string Email
        {
            get => _email;
            set => _email = SantizeData.SanitizeString(value);
        }
    }
}
