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

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing != null)
            {
                _context.Listings.Remove(listing);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
