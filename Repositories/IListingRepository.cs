using Rems_Auth.Models;

namespace Rems_Auth.Repositories
{
    public interface IListingRepository
    {
        Task<AddListing> AddListingAsync(AddListing listing);
        Task<IEnumerable<AddListing>> GetAllListingsAsync();
        Task<AddListing?> GetListingByIdAsync(int id);
        Task<IEnumerable<AddListing>> GetListingsByUserAsync(int userId);

        
        Task<AddListing?> UpdateListingAsync(AddListing listing);
        Task<bool> DeleteListingAsync(int id);
    }
}
