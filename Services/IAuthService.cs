using Rems_Auth.Dtos;
using Rems_Auth.Models;

namespace Rems_Auth.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
        Task<AuthResponse> SignupAsync(SignupRequest signupRequest);
        Task ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
        Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
        Task<User> ValidateRefreshTokenAsync(string refreshToken);
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
