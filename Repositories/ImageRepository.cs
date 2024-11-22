using Rems_Auth.Data;
using Microsoft.EntityFrameworkCore;

namespace Rems_Auth.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Image> AddImageAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<List<Image>> GetImagesByListingIdAsync(Guid listingId)
        {
            return await _context.Images.Where(i => i.ListingId == listingId).ToListAsync();
        }
    }
}
