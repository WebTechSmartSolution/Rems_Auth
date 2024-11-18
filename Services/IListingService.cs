using Rems_Auth.Dtos;

namespace Rems_Auth.Services
{
    public interface IListingService
    {
        Task<ListingResponse> AddListingAsync(ListingRequest request);
        Task<IEnumerable<ListingResponse>> GetAllListingsAsync();
        Task<IEnumerable<ListingResponse>> GetListingsByUserAsync(int userId);

        Task<ListingResponse?> GetListingByIdAsync(int id);
        Task<bool> DeleteListingAsync(int id);
    }
}
