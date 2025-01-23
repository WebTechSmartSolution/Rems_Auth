using Microsoft.IdentityModel.Tokens;
using Rems_Auth.Repositories;
using Rems_Auth.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Rems_Auth.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            try
            {
                var tokenService = context.RequestServices.GetRequiredService<ITokenService>();
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    // Check if the token is for an admin or user
                    var tokenData = ValidateTokenBasedOnRole(tokenService, token);

                    if (tokenData != null)
                    {
                        // Set the user or admin to context
                        if (tokenData is (string userEmail, Guid userId))
                        {
                            var user = await userRepository.GetUserByIdAsync(userId);
                            if (user == null)
                            {
                                context.Response.StatusCode = StatusCodes.Status404NotFound;
                                await context.Response.WriteAsync("User not found.");
                                return;
                            }
                            context.Items["User"] = user;
                        }
                        else if (tokenData is (string adminUsername, Guid adminId))
                        {
                            // Add admin handling logic if needed (e.g., setting admin in context)
                            context.Items["Admin"] = new { adminUsername, adminId };
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid token");
                        return;
                    }
                }

                await _next(context);
            }
            catch (SecurityTokenException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync($"Token error: {ex.Message}");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"Unexpected error: {ex.Message}");
            }
        }

        private object ValidateTokenBasedOnRole(ITokenService tokenService, string token)
        {
            // Try to validate as a user token first
            var userData = tokenService.ValidateUserToken(token);
            if (userData.HasValue)
                return userData.Value;

            // If user validation fails, try admin token validation
            var adminData = tokenService.ValidateAdminToken(token);
            if (adminData.HasValue)
                return adminData.Value;

            return null;
        }
    }
}
