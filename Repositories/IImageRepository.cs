namespace Rems_Auth.Repositories
{
    public interface IImageRepository
    {
        Task<Image> AddImageAsync(Image image);
        Task<List<Image>> GetImagesByListingIdAsync(Guid listingId);
    }

}
