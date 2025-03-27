using Microsoft.Extensions.Options; 
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims; 

namespace BlogsAPI.Services
{
    public class UserService : IUserService<AppUser>
    { 
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        private IOptions<AppSettings> _appsettings;
        private readonly ICommonRepository<Contact> _contactCommonRepository; 
        public UserService( 
            UserManager<AppUser> manager,
            RoleManager<IdentityRole> roleManager, 
            IOptions<AppSettings> appsettings,
            ICommonRepository<Contact> contactCommonRepository
            )
        { 
            _userManager = manager;
            _appsettings = appsettings;
            _roleManager = roleManager; 
            _contactCommonRepository = contactCommonRepository; 
        }

        public async Task<ServiceResult> RegiserationAsync(RegisterDto usercredintials)
        {
            var user = await _userManager.FindByEmailAsync(usercredintials.Email);

            if (user is not null) return ServiceResult.Failure("User is already exists");

            AppUser newuser = usercredintials.FromAppUserDto();

            var result = await _userManager.CreateAsync(newuser, usercredintials.Password!); 

            if (!result.Succeeded)
                return ServiceResult.Failure("User couldn't be created");

            var userdata = await _userManager.FindByEmailAsync(usercredintials.Email);

            if (usercredintials.Roles.Any(x => x.Contains(AppConstants.ADMIN)))
                usercredintials.Roles.Remove(AppConstants.ADMIN); 
             
            await AssingRoles(usercredintials.Roles, user);

            //await _mailing.SendMail(user.Email, "New Account Confirmation", "We confirm you that a new account was created using this email", " ");
           
            return ServiceResult.Success("User is created successfully");
        }

        public async Task<ServiceResult> LoginAsync(LoginDto usercredintials)
        {
            var user = await _userManager.FindByEmailAsync(usercredintials.Email);
            var password = await _userManager.CheckPasswordAsync(user, usercredintials.Password);

            if (user is null || !password) return ServiceResult.Failure("Incorrect email or password");

            var token = await GenerateJwtToken(user);

            //await _mailing.SendMail(user.Email, "Login Confirmation", "We confirm you that you have logged in at :", DateTime.Now.ToString());

            return ServiceResult.Success(token);
        }
         
        public async Task AssingRoles(List<string> roles, AppUser user)
        {
            foreach (var role in roles)
            {
                var isInRole = await _userManager.IsInRoleAsync(user, role);
                if (!isInRole && await _roleManager.RoleExistsAsync(role))
                    await _userManager.AddToRoleAsync(user, role);
            }
        } 

        private async Task<string?> GenerateJwtToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(_appsettings.Value.JWTSecret);
            var signingKey = new SymmetricSecurityKey(key);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
             
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _appsettings.Value.JWTIssuer,
                Audience = _appsettings.Value.JWTAudience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
         
        private async Task<string?> GenerateNewPasswordAsync(AppUser user, string newpassword)
        {
            var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resut = await _userManager.ResetPasswordAsync(user, passwordToken, newpassword);
            if(resut is null) return null;
            return "success";
        }

        public async Task<GetUsersDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return user.GetAllUsersDto(roles.ToList()); 
        }

        public async Task<AppUser?> GetOrigianlUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return null; 

            return user;
        }

        public async Task<List<GetUsersDto>?> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users is null)
                return null;

            List<GetUsersDto> usersDto = new();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersDto.Add(user.GetAllUsersDto(roles.ToList()));
            }

            return usersDto; 
        }

        public async Task<string?> GetUsernameAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            if (result != null)
                return result.UserName;

            return null;
        }

        public async Task<ServiceResult> ConfirmEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result != null)
            {
                int otp = Random.Shared.Next(1000, 9000);

                //await _mailing.SendMail(email, "Email Confirmation", "We confirm you that your Otp to enter is :", otp.ToString());

                return ServiceResult.Success("Confirmed successfully");
            }
            else
                return ServiceResult.Failure($"No user found using this email {email}");
        }

        public async Task<ServiceResult> UpdatePasswordAsync(string email, string password)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var result = await GenerateNewPasswordAsync(user, password);
                if (result is not null)
                {
                    //await _mailing.SendMail(email, "Update Password Confirmation", "We confirm you that you have changed your password at :", DateTime.Now.ToString());

                    return ServiceResult.Success("a new password was reseted successfully");
                }
            }
            return ServiceResult.Failure($"No user found using this email {email}");
        }

        public async Task<ServiceResult> ChangeUsernameAsync(string email, string newusername)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.UserName = newusername;
                var result = await _userManager.UpdateAsync(user);
                if (result is not null)
                {
                    //await _mailing.SendMail(email, "Update Username Confirmation", $"We confirm you that you have changed your username to {newusername} at :", DateTime.Now.ToString());

                    return ServiceResult.Success(newusername);
                }
            }
            return ServiceResult.Failure($"No user found using this email {email}");
        }

        public async Task<ServiceResult> ContactUsAsync(ContactDto data)
        {
            var user = await _userManager.FindByEmailAsync(data.Email);
            if (user is not null)
            {
                Contact obj = new();
                obj.AppUserId = user.Id;
                obj.Message = data.Message;
                var result = await _contactCommonRepository.AddAsync(obj);
                if (result is not null)
                    return ServiceResult.Success("successfull operation");
            }
            return ServiceResult.Failure($"No user found using this email {data.Email}");
        }
        
        public async Task<ServiceResult> NotifyUserStatusAsync(string email)
        { 
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null || user.Notify == false)
            {
                user.Notify = true;
                var result = await _userManager.UpdateAsync(user);
                if (result is not null)
                    return ServiceResult.Success("successfull operation");
            }

            return ServiceResult.Failure($"No user found using this email {email}");
        }

        public async Task<ServiceResult> DeleteUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result is not null)
                    return ServiceResult.Success("Deleted successfully");

                return ServiceResult.Failure("Something went wrong");

            }
            return ServiceResult.Failure($"No user found using this id {id}");
        }
    }
}
