using Rems_Auth.Dtos;
using Rems_Auth.Models;

namespace Rems_Auth.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<int> GetTotalUsersAsync();
        // IUserService.cs
        Task<User> UpdateUserAsync(Guid userId, UserUpdateRequest request);

        Task UpdateUserProfilePictureAsync(Guid userId, IFormFile profilePicture);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
