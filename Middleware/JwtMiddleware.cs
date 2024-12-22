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
                    var tokenData = tokenService.ValidateToken(token);

                    if (tokenData.HasValue)
                    {
                        var userId = tokenData.Value.userId;

                        var user = await userRepository.GetUserByIdAsync(userId);

                        if (user == null)
                        {
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            await context.Response.WriteAsync("User not found.");
                            return;
                        }

                        context.Items["User"] = user;
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

    }
}
