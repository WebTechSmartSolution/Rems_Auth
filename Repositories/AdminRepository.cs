﻿using Rems_Auth.Data;
using Rems_Auth.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Rems_Auth.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task AddUserAsync(Admin user)
        {
            user.CreatedAt = DateTime.UtcNow;
            await _context.Admins.AddAsync(user);
        }

        public async Task<Admin> GetUserByUsernameAsync(string username)
        {
            return await _context.Admins.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task UpdateAdminAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }
    }
}
