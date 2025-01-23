using Rems_Auth.Dtos;
using Rems_Auth.Models;
using System.Reflection;

namespace Rems_Auth.Repositories
{
    public interface IListingRepository
    {
        Task<AddListing> AddListingAsync(AddListing listing);
        Task<List<AddListing>> GetAllListingsAsync();
        Task<AddListing> GetListingByIdAsync(Guid id);
        Task<List<AddListing>> GetListingsByUserIdAsync(Guid userId);  // Get listings by user ID
        Task<AddListing> UpdateListingAsync(AddListing listing);
        Task<IEnumerable<Review>> GetReviewsByListingIdAsync(Guid listingId);
        Task<Review> AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<bool> DeleteReviewAsync(Guid listingId, Guid reviewId);
        //Task<IEnumerable<AddListing>> GetListingsByUserIdAsync(Guid userId);
        Task DeleteListingAsync(AddListing listing);
        Task<bool> DeleteListingAsync(Guid id);
    }
}
