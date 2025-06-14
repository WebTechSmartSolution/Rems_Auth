using Microsoft.AspNetCore.Mvc;
using Rems_Auth.Dtos;
using Rems_Auth.Services;
using System.Threading.Tasks;

namespace Rems_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] AdminSignUpRequest request)
        {
            try
            {
                var response = await _adminService.SignupAsync(request);
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginRequest request)
        {
            try
            {
                var token = await _adminService.AuthenticateAsync(request);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("change_password")]
        public async Task<IActionResult> ChangeAdminPassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var success = await _adminService.ChangePasswordAsync(request);
                return Ok(new { Message = "Password changed successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}
