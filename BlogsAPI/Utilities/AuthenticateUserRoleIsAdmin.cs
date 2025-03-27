using System.Security.Claims;

namespace BlogsAPI.Utilities
{
    public static class AuthenticateUserRoleIsAdmin
    {
        public static bool IsAdmin(ClaimsPrincipal claims)
        { 
            var roles = claims.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return roles.Any(x=>x.Equals(AppConstants.ADMIN));
        }
    }
}
