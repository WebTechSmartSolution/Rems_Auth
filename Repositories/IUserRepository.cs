using Rems_Auth.Models;

namespace Rems_Auth.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByResetTokenAsync(string resetToken);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task UpdateUserProfilePictureAsync(Guid userId, string profilePictureUrl);
        Task SaveChangesAsync();
        Task InvalidateUserRefreshTokenAsync(User user);
    }
}
