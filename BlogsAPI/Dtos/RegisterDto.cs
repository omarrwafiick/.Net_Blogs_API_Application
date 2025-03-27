using System.ComponentModel.DataAnnotations; 

namespace BlogsAPI.Dtos
{
    public class RegisterDto
    {  
        private string _userName;
        private string _fullName;
        private string _email;
        private string _phone;
        private string _password;
        private List<string> _roles;

        [MinLength(6)]
        [MaxLength(30)]
        public string UserName
        {
            get => _userName;
            set => _userName = SantizeData.SanitizeString(value);
        }

        [MinLength(12)]
        [MaxLength(32)] 
        public string FullName
        {
            get => _fullName;
            set => _fullName = SantizeData.SanitizeString(value);
        }

        [MinLength(7)]
        [MaxLength(254)]
        [EmailAddress]
        public string Email
        {
            get => _email;
            set => _email = SantizeData.SanitizeString(value);
        }

        [MinLength(8)]
        [MaxLength(15)]
        public string Phone
        {
            get => _phone;
            set => _phone = SantizeData.SanitizeString(value);
        }
        public string Password
        {
            get => _password;
            set => _password = SantizeData.SanitizeString(value);
        }
         
        public bool Gender { get; set; }

        public List<string> Roles
        {
            get =>  _roles ;
            set =>  _roles = SanitizeCollection(value);
        }

       private List<string> SanitizeCollection(List<string> rolesList)
        {
            List<string> newArray = new();
            foreach (var role in rolesList)
            {
                newArray.Add(SantizeData.SanitizeString(role));
            }
            return newArray;
        }
    }
}
