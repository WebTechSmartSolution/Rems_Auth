using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rems_Auth.Dtos;
using Rems_Auth.Models;
using Rems_Auth.Services;

namespace Rems_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                CountryCode = user.CountryCode,
                ProfileImageUrl = user.ProfilePictureUrl,
                IsAgent = user.IsAgent,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(userResponse);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromForm] UserUpdateRequest request)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, request);

                var userResponse = new UserResponse
                {
                    Id = updatedUser.Id,
                    Name = updatedUser.Name,
                    Email = updatedUser.Email,
                    MobileNumber = updatedUser.MobileNumber,
                    CountryCode = updatedUser.CountryCode,
                    ProfileImageUrl = updatedUser.ProfilePictureUrl,
                    IsAgent = updatedUser.IsAgent,
                    CreatedAt = updatedUser.CreatedAt,
                    UpdatedAt = updatedUser.UpdatedAt
                };

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}/profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture(Guid id, [FromForm] IFormFile profilePicture)
        {
            try
            {
                await _userService.UpdateUserProfilePictureAsync(id, profilePicture);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
