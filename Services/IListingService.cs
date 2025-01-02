using Rems_Auth.Dtos;

namespace Rems_Auth.Services
{
    public interface IListingService
    {
        Task<ListingResponse> CreateListingAsync(ListingRequest request, Guid userId);
        Task<List<ListingResponse>> GetAllListingsAsync();
        Task<ListingResponse> GetListingByIdAsync(Guid id);
        Task<List<ListingResponse>> GetListingsByUserIdAsync(Guid userId);
        Task<ListingResponse> UpdateListingAsync(Guid id, UpdateListingRequest request);
        Task<ListingResponse> ChangeListingStatusAsync(Guid id);
        Task<IEnumerable<ReviewResponse>> GetReviewsByListingIdAsync(Guid listingId);
        Task<ReviewResponse> AddReviewAsync(Guid listingId, ReviewRequest request);
        Task<bool> DeleteReviewAsync(Guid listingId, Guid reviewId);
        Task<bool> DeleteListingAsync(Guid id);
    }
}
