using System.Threading.Tasks;
using Rems_Auth.Models;

namespace Rems_Auth.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByUsernameAsync(string username);
        Task UpdateAdminAsync(Admin admin);
    }
}
