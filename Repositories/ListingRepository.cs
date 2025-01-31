using Microsoft.EntityFrameworkCore;
using Rems_Auth.Data;
using Rems_Auth.Dtos;
using Rems_Auth.Models;
using System.Reflection;

namespace Rems_Auth.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly ApplicationDbContext _context;

        public ListingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AddListing> AddListingAsync(AddListing listing)
        {
            await _context.Listings.AddAsync(listing);
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task<List<AddListing>> GetAllListingsAsync()
        {
            return await _context.Listings.Include(l => l.Images).ToListAsync();  // Include Images
        }

        public async Task<AddListing> GetListingByIdAsync(Guid id)  // Change Guid to int if needed
        {
            return await _context.Listings.Include(l => l.Images).FirstOrDefaultAsync(l => l.Id == id);
        }


        public async Task<List<AddListing>> GetListingsByUserIdAsync(Guid userId)
        {
            return await _context.Listings
                                 .Where(l => l.UserId == userId)
                                 .Include(l => l.Images)
                                 .ToListAsync();  // Filter listings by user ID and include images
        }

        public async Task<AddListing> UpdateListingAsync(AddListing listing)
        {
            _context.Listings.Update(listing);
            await _context.SaveChangesAsync();
            return listing;
        }
        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(Guid listingId, Guid reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.ListingId == listingId);
            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Review>> GetReviewsByListingIdAsync(Guid listingId)
        {
            return await _context.Reviews
                .Where(r => r.ListingId == listingId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            var listing = await _context.Listings
                .Include(l => l.Images)
                .Include(l => l.Chats) // Include related chats
                .ThenInclude(c => c.Messages) // Include messages of those chats
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing != null)
            {
                // Remove associated messages and chats
                foreach (var chat in listing.Chats)
                {
                    _context.Messages.RemoveRange(chat.Messages); // Remove chat messages
                    _context.Chats.Remove(chat); // Remove chats
                }

                // Remove associated images
                _context.Images.RemoveRange(listing.Images);

                // Remove the listing
                _context.Listings.Remove(listing);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task DeleteListingAsync(AddListing listing)
        {
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();
        }
    }
}
