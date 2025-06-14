using Microsoft.AspNetCore.Identity;
using Rems_Auth.Dtos;
using Rems_Auth.Models;
using Rems_Auth.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rems_Auth.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<Admin> _passwordHasher;
        public AdminService(IAdminRepository adminRepository, ITokenService tokenService)
        {
            _adminRepository = adminRepository;
            _tokenService = tokenService;
        }

        public async Task<string> AuthenticateAsync(AdminLoginRequest request)
        {
            var admin = await _adminRepository.GetAdminByUsernameAsync(request.Username);
            if (admin == null || !VerifyPassword(request.Password, admin.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            // Generate a JWT token using the TokenService
            return _tokenService.GenerateTokenForAdmin(admin);
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var admin = await _adminRepository.GetAdminByUsernameAsync("admin"); // Assume default admin for now
            if (admin == null || !VerifyPassword(request.OldPassword, admin.PasswordHash))
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }

            admin.PasswordHash = HashPassword(request.NewPassword);
            await _adminRepository.UpdateAdminAsync(admin);
            return true;
        }

        public async Task<AdminResponce> SignupAsync(AdminSignUpRequest request)
        {
            try
            {
                var existingUser = await _adminRepository.GetUserByUsernameAsync(request.Username);
                if (existingUser != null)
                {
                    return new AdminResponce { Message = "Email is already registered." };
                }

                var user = new Admin
                {
                    Username = request.Username,
                    PasswordHash =  request.Password
                };

                await _adminRepository.AddUserAsync(user);
                await _adminRepository.SaveChangesAsync();

                return new AdminResponce { Message = "Registration successful." };
            }
            catch (Exception ex)
            {
                return new AdminResponce { Message = $"An error occurred during signup: {ex.Message}" };
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}
