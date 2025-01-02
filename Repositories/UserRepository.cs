using Microsoft.EntityFrameworkCore;
using Rems_Auth.Data;
using Rems_Auth.Models;
using System;

namespace Rems_Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByResetTokenAsync(string resetToken)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.ResetToken == resetToken);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
        }

        public async Task UpdateUserProfilePictureAsync(Guid userId, string profilePictureUrl)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                user.ProfilePictureUrl = profilePictureUrl;
                user.UpdatedAt = DateTime.UtcNow;
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task InvalidateUserRefreshTokenAsync(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpires = DateTime.MinValue;
            await SaveChangesAsync();
        }
    }
}
