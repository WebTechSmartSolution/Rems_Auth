using Rems_Auth.Dtos;

namespace Rems_Auth.Services
{
    public interface IAdminService
    {
        Task<string> AuthenticateAsync(AdminLoginRequest request);
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
