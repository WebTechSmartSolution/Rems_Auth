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
            try
            {
                return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user by email", ex);
            }
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            try
            {
                return await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user by ID", ex);
            }
        }

        public async Task<User> GetUserByResetTokenAsync(string hashedResetToken)
        {
            try
            {
                return await _context.Users.SingleOrDefaultAsync(u => u.ResetToken == hashedResetToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user by reset token", ex);
            }
        }
        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                return await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the refresh token", ex);
            }
        }
       
        public async Task InvalidateUserRefreshTokenAsync(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpires = DateTime.MinValue;
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user by ID", ex);
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                user.CreatedAt = DateTime.UtcNow;
                await _context.Users.AddAsync(user);
                await _context.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                user.UpdatedAt = DateTime.UtcNow;
                _context.Users.Update(user);
                _context.Users.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes to the database", ex);
            }
        }
    }
}
