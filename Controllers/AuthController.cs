using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rems_Auth.Services;
using System.Threading.Tasks;
using Rems_Auth.Dtos;
using System;
using Rems_Auth.Repositories;

namespace Rems_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService , IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _authService.LoginAsync(loginRequest);
            if (response.Message == "Invalid email or password.")
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpPost("refresh-Token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (string.IsNullOrEmpty(tokenRequest.RefreshToken))
            {
                return BadRequest("Invalid request");
            }

            var user = await _authService.ValidateRefreshTokenAsync(tokenRequest.RefreshToken);
            if (user == null)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            var newAccessToken = _authService.GenerateAccessToken(user);
            var newRefreshToken = _authService.GenerateRefreshToken();

            // Update the refresh token in the database
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpires = DateTime.UtcNow.AddDays(1); // Example: refresh token valid for 7 days
            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Message = "Token refreshed successfully."
            });
        }



        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            try
            {
                var response = await _authService.SignupAsync(request);
                if (response.Message == "Email is already registered.")
                {
                    return Conflict(new { Message = response.Message }); // 409 Conflict for existing email
                }

                return Ok(response); // 200 OK for successful registration
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred during signup", Details = ex.Message });
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                await _authService.ForgotPasswordAsync(request);
                return Ok(new { Message = "Password reset link sent." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred: ", Details = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var response = await _authService.ResetPasswordAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred: ", Details = ex.Message });
            }
        }
    }
}
