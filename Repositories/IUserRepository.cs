using Rems_Auth.Models;

namespace Rems_Auth.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByResetTokenAsync(string resetToken);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken); // New method
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task SaveChangesAsync();
        Task InvalidateUserRefreshTokenAsync(User user);
    }
}
