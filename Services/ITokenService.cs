using Rems_Auth.Models;

namespace Rems_Auth.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        (string userEmail, Guid userId)? ValidateToken(string token); // Existing method

        string GenerateTokenForAdmin(Admin admin); // Existing method

        // Add these methods to the interface
        (string userEmail, Guid userId)? ValidateUserToken(string token);
        (string adminUsername, Guid adminId)? ValidateAdminToken(string token);
    }
}
