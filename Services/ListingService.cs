using Rems_Auth.Dtos;
using Rems_Auth.Models;
using Rems_Auth.Repositories;


namespace Rems_Auth.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;
        private readonly IImageRepository _imageRepository;

        public ListingService(IListingRepository listingRepository, IImageRepository imageRepository)
        {
            _listingRepository = listingRepository;
            _imageRepository = imageRepository;
        }

        public async Task<ListingResponse> CreateListingAsync(ListingRequest request, Guid userId)
        {
            // Create a new Listing entity
            var listing = new AddListing
            {
                 PropertyName = request.PropertyName,
    PropertyType = request.PropertyType,
    CurrencyType = request.CurrencyType,
    SalePrice = request.SalePrice,
    OfferPrice = request.OfferPrice,

    
    PropertyId = request.PropertyId,
    PricePerSqft = request.PricePerSqft,
    NoOfBedrooms = request.NoOfBedrooms,
    NoOfBathrooms = request.NoOfBathrooms,
    Sqft = request.Sqft,
    NoOfFloors = request.NoOfFloors,
    GarageSize = request.GarageSize,
    YearConstructed = request.YearConstructed,

 
    Address = request.Address,
    City = request.City,
    State = request.State,
    ZipCode = request.ZipCode,

    
    UserId = userId, // Associate the listing with the user
    CreatedAt = DateTime.UtcNow

                
            };

            // Save the listing to the database
            var createdListing = await _listingRepository.AddListingAsync(listing);

            // Handle image uploads
            var images = new List<Image>();
            if (request.Images != null && request.Images.Count > 0)
            {
                foreach (var image in request.Images)
                {
                    // Save the image to a directory and get its path
                    var imagePath = await SaveImageAsync(image);

                    // Create an Image entity
                    var imageEntity = new Image
                    {
                        ListingId = createdListing.Id,
                        Path = imagePath
                    };

                    // Save each image entity to the database using AddImageAsync
                    var savedImage = await _imageRepository.AddImageAsync(imageEntity);

                    // Add the saved image to the images list
                    images.Add(savedImage);
                }
            }

            // Map the created listing and associated images to a response DTO
            return new ListingResponse
            {
                Id = createdListing.Id,
                PropertyName = createdListing.PropertyName,
                Description = createdListing.Description,
                Address = createdListing.Address,
                SalePrice = createdListing.SalePrice,
                Images = images.Select(i => new ImageResponse { Path = i.Path }).ToList(),
                CreatedAt = createdListing.CreatedAt
            };
        }

        // Private helper method to save images to the server
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            // Define the directory to save images
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            Directory.CreateDirectory(uploadsFolder);

            // Create a unique file name for the image
            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the image file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Return the relative path to the saved image
            return $"/images/{uniqueFileName}";
        }

        public async Task<List<ListingResponse>> GetAllListingsAsync()
        {
            var listings = await _listingRepository.GetAllListingsAsync();
            return listings.Select(l => new ListingResponse
            {
                Id = l.Id,
                PropertyName = l.PropertyName,
                Description = l.Description,
                Address = l.Address,
                SalePrice = l.SalePrice,
                Images = l.Images.Select(i => new ImageResponse { Path = i.Path }).ToList()
            }).ToList();
        }

        public async Task<ListingResponse> GetListingByIdAsync(Guid id)
        {
            var listing = await _listingRepository.GetListingByIdAsync(id);
            return new ListingResponse
            {
                Id = listing.Id,
                PropertyName = listing.PropertyName,
                Description = listing.Description,
                Address = listing.Address,
                SalePrice = listing.SalePrice,
                Images = listing.Images.Select(i => new ImageResponse { Path = i.Path }).ToList()
            };
        }

        public async Task<List<ListingResponse>> GetListingsByUserIdAsync(Guid userId)
        {
            var listings = await _listingRepository.GetListingsByUserIdAsync(userId);
            return listings.Select(l => new ListingResponse
            {
                Id = l.Id,
                PropertyName = l.PropertyName,
                Description = l.Description,
                Address = l.Address,
                SalePrice = l.SalePrice,
                Images = l.Images.Select(i => new ImageResponse { Path = i.Path }).ToList()
            }).ToList();
        }

        public async Task<ListingResponse> UpdateListingAsync(Guid id, UpdateListingRequest request)
        {
            var listing = new AddListing
            {
                Id = id,
                PropertyName = request.PropertyName,
                PropertyId = request.PropertyId,
                Description = request.Description,
                Address = request.Address,
                SalePrice = request.SalePrice
            };

            var updatedListing = await _listingRepository.UpdateListingAsync(listing);
            return new ListingResponse
            {
                Id = updatedListing.Id,
                PropertyName = updatedListing.PropertyName,
                Description = updatedListing.Description,
                Address = updatedListing.Address,
                SalePrice = updatedListing.SalePrice,
                Images = updatedListing.Images.Select(i => new ImageResponse { Path = i.Path }).ToList()
            };
        }

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            return await _listingRepository.DeleteListingAsync(id);
        }
    }

}
