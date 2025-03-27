using Microsoft.AspNetCore.Mvc; 
namespace BlogsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {  
        private readonly IUserService<AppUser> _usersService;   
        public UsersController(IUserService<AppUser> userService)
        {
            _usersService = userService;   
        }

        [HttpPost("register")] 
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto user)
        {
            var result = await _usersService.RegiserationAsync(user);  

            return !result.SuccessOrNot ? BadRequest(result.Message): Ok(result.Message);
        }

        [HttpPost("login")] 
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto user)
        {
            var result = await _usersService.LoginAsync(user);

            return !result.SuccessOrNot ? BadRequest(result.Message) : Ok(result.Message);
        }

        [HttpGet("getuserbyid")] 
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            var result = await _usersService.GetUserByIdAsync(id); 

            return result is not null ? Ok(result) : NotFound($"No user found using this id {id}"); 
        }
         
        [HttpGet("getallusers")] 
        public async Task<IActionResult> GetAllUsers()
        {
            var isAdmin = AuthenticateUserRoleIsAdmin.IsAdmin(HttpContext.User);

            if(!isAdmin)
                return Unauthorized("User is not in role");

            var result = await _usersService.GetAllUsersAsync(); 
             
            return result is not null ? Ok(result) : NotFound("No user was found");
        } 

        [HttpGet("getusername")]
        public async Task<IActionResult> GetUsername([FromQuery] string id)
        {
            var result = await _usersService.GetUsernameAsync(id); 
             
            return result is not null ? Ok(result) : NotFound($"No user found using this id {id}");
        }

        [HttpGet("confirmemail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email)
        {
            var result = await _usersService.ConfirmEmailAsync(email);

            return result.SuccessOrNot? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("assigntoroles")]
        public async Task<IActionResult> AssignRoles([FromBody] List<string> roles, [FromRoute] string userid)
        {
            var isAdmin = AuthenticateUserRoleIsAdmin.IsAdmin(HttpContext.User);

            if (!isAdmin)
                return Unauthorized("User is not in role");

            var user = await _usersService.GetOrigianlUserByIdAsync(userid);

            if (user is null)
                return BadRequest("User was not found");

            await _usersService.AssingRoles(roles, user);

            return NoContent();
        }

        [HttpPost("updatepassword")] 
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword([FromQuery] string email,[FromBody] string password)
        {
            var result = await _usersService.UpdatePasswordAsync(email, password);
             
            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("changeusername")] 
        public async Task<IActionResult> ChangeUsername([FromQuery] string email,[FromBody] string newusername)
        {
            var result = await _usersService.ChangeUsernameAsync(email, newusername);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("contactus")] 
        public async Task<IActionResult> ContactUs([FromBody] ContactDto data)
        {
            var result = await _usersService.ContactUsAsync(data);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("notifyuserstatus")] 
        public async Task<IActionResult> NotifyUserStatus([FromBody] string email)
        {
            var result = await _usersService.NotifyUserStatusAsync(email);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUserById([FromQuery] string id)
        {
            var isAdmin = AuthenticateUserRoleIsAdmin.IsAdmin(HttpContext.User);

            if (!isAdmin)
                return Unauthorized("User is not in role");

            var result = await _usersService.DeleteUserByIdAsync(id);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        } 
    }
}
