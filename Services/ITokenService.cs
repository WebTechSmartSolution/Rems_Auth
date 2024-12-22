using Rems_Auth.Models;

namespace Rems_Auth.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateTokenForAdmin(Admin admin); // New method
        (string userEmail, Guid userId)? ValidateToken(string token);
    }
}
