using Microsoft.EntityFrameworkCore;
using Rems_Auth.Data;
using Rems_Auth.Models;

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
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task<IEnumerable<AddListing>> GetAllListingsAsync()
        {
            return await _context.Listings
                .Include(l => l.User) // Include User relationship
                .ToListAsync();
        }

        public async Task<AddListing?> GetListingByIdAsync(int id)
        {
            return await _context.Listings
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        
        public async Task<IEnumerable<AddListing>> GetListingsByUserAsync(int userId)
        {
            return await _context.Listings
                .Where(listing => listing.UserId == userId)
                .ToListAsync();
        }

        public async Task<AddListing?> UpdateListingAsync(AddListing listing)
        {
            var existingListing = await _context.Listings.FindAsync(listing.Id);
            if (existingListing == null) return null;

            _context.Entry(existingListing).CurrentValues.SetValues(listing);
            await _context.SaveChangesAsync();

            return existingListing;
        }

        public async Task<bool> DeleteListingAsync(int id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing == null) return false;

            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
