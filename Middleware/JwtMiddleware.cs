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
                    var userEmail = tokenService.ValidateToken(token);
                    if (userEmail != null)
                    {
                        context.Items["User"] = await userRepository.GetUserByEmailAsync(userEmail);
                    }
                }

                await _next(context);
            }
            catch (SecurityTokenException ex)
            {
                // Log and handle token validation errors
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
