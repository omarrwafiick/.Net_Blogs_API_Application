namespace BlogsAPI.Interfaces
{ 
    public interface IUserService<T> : IScopedService<T> where T : class
    {
        Task<ServiceResult> RegiserationAsync(RegisterDto userdata);
        Task<ServiceResult> LoginAsync(LoginDto userdata);
        Task AssingRoles(List<string> roles, AppUser user);
        Task<GetUsersDto?> GetUserByIdAsync(string id);
        Task<AppUser?> GetOrigianlUserByIdAsync(string id);
        Task<List<GetUsersDto>?> GetAllUsersAsync();
        Task<string?> GetUsernameAsync(string id);
        Task<ServiceResult> ConfirmEmailAsync(string email);
        Task<ServiceResult> UpdatePasswordAsync(string email, string password);
        Task<ServiceResult> ChangeUsernameAsync(string email, string newusername);
        Task<ServiceResult> ContactUsAsync(ContactDto data);
        Task<ServiceResult> NotifyUserStatusAsync(string email);
        Task<ServiceResult> DeleteUserByIdAsync(string id);
    }
}
