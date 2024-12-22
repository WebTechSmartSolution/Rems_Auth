using Rems_Auth.Dtos;
using Rems_Auth.Models;
using Rems_Auth.Repositories;
using Rems_Auth.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Rems_Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IEmailService emailService, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                {
                    return new AuthResponse { Message = "Invalid email or password." };
                }

                // Generate JWT token
                var accessToken = _tokenService.GenerateToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpires = DateTime.UtcNow.AddDays(7); // Refresh token valid for 7 days

                await _userRepository.UpdateUserAsync(user);
                await _userRepository.SaveChangesAsync();

                return new AuthResponse
                {
                    Token = accessToken,
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                    RefreshToken = refreshToken,
                    Email = user.Email,
                    RefreshTokenExpires = user.RefreshTokenExpires.Value,
                    Message = "Authentication successful."
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = $"An error occurred during login: {ex.Message}" };
            }
        }

        public async Task<AuthResponse> SignupAsync(SignupRequest request)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new AuthResponse { Message = "Email is already registered." };
                }

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    MobileNumber = request.MobileNumber,
                    CountryCode = request.CountryCode ?? "+92",  // Default to Pakistan country code if null
                    IsAgent = request.IsAgent ?? false,  // Default to false if IsAgent is null
                    PasswordHash = _passwordHasher.HashPassword(null, request.Password)
                };

                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangesAsync();

                return new AuthResponse { Message = "Registration successful." };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = $"An error occurred during signup: {ex.Message}" };
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("Email not registered.");
                }

                var resetToken = GenerateSecureToken();
                var hashedResetToken = HashToken(resetToken);

                user.ResetToken = hashedResetToken;
                user.ResetTokenExpires = DateTime.UtcNow.AddHours(2);

                await _userRepository.UpdateUserAsync(user);
                await _userRepository.SaveChangesAsync();

                var resetLink = $"http://localhost:5173/Reset-Password?token={resetToken}";
                await _emailService.SendPasswordResetEmailAsync(user.Email, "Reset Password", $"Click <a href='{resetLink}'>here</a> to reset your password.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while sending the reset password link: {ex.Message}", ex);
            }
        }

        public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var hashedToken = HashToken(request.Token);
                var user = await _userRepository.GetUserByResetTokenAsync(hashedToken);

                if (user == null || user.ResetTokenExpires == null || user.ResetTokenExpires < DateTime.UtcNow)
                {
                    return new AuthResponse { Message = "Invalid or expired reset token." };
                }

                user.PasswordHash = _passwordHasher.HashPassword(null, request.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpires = null;

                await _userRepository.UpdateUserAsync(user);
                await _userRepository.SaveChangesAsync();

                return new AuthResponse { Message = "Password has been reset successfully." };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Message = $"An error occurred while resetting the password: {ex.Message}" };
            }
        }

        // Validate refresh token logic
        public async Task<User> ValidateRefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpires < DateTime.UtcNow)
            {
                return null;  // Refresh token is invalid or expired
            }

            return user;
        }

        // Generate JWT Access Token
        //public string GenerateAccessToken(User user)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Convert.FromBase64String(_jwtSettings.Secret);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Email),
        //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}

        // Generate Refresh Token
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        private static string GenerateSecureToken(int length = 32)
        {
            var tokenBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes);
        }

        private static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
