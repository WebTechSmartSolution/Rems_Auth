using Azure.Core;
using Microsoft.EntityFrameworkCore;
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
                 status = request.status,
    
                 PropertyId = request.PropertyId,
                 PricePerSqft = request.PricePerSqft,
                 NoOfBedrooms = request.NoOfBedrooms,
                 NoOfBathrooms = request.NoOfBathrooms,
                 Sqft = request.Sqft,
                 NoOfFloors = request.NoOfFloors,
                 GarageSize = request.GarageSize,
                 YearConstructed = request.YearConstructed,
                 Description = request.Description,
                 Email = request.Email,
                 Phone = request.Phone,

 
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
                OwnerId = createdListing.UserId,
                PropertyName = createdListing.PropertyName,
                 PropertyType = createdListing.PropertyType,
                 CurrencyType = createdListing.CurrencyType,
                 SalePrice = createdListing.SalePrice,
                 OfferPrice = createdListing.OfferPrice,
                status = createdListing.status,
                // Property Details
                PropertyId = createdListing.PropertyId,
                 PricePerSqft = createdListing.PricePerSqft,
                 NoOfBedrooms = createdListing.NoOfBedrooms,
                 NoOfBathrooms = createdListing.NoOfBathrooms,
                 Sqft = createdListing.Sqft,
                 NoOfFloors = createdListing.NoOfFloors,
                 GarageSize = createdListing.GarageSize,
                 YearConstructed = createdListing.YearConstructed,
                Description = createdListing.Description,
                Email = createdListing.Email,
                 Phone = createdListing.Phone,

                  // Location
                Address = createdListing.Address,
                City = createdListing.City,
                State = createdListing.State,
                ZipCode = createdListing.ZipCode,


                
                Images = images.Select(i => new ImageResponse { Path = i.Path }).ToList(),
                CreatedAt = createdListing.CreatedAt,
                UpdatedAt = createdListing.UpdatedAt
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
                OwnerId = l.UserId,
                PropertyName = l.PropertyName,
                PropertyType = l.PropertyType,
                CurrencyType = l.CurrencyType,
                SalePrice = l.SalePrice,
                OfferPrice = l.OfferPrice,
                status = l.status,
                // Property Details
                PropertyId = l.PropertyId,
                PricePerSqft = l.PricePerSqft,
                NoOfBedrooms = l.NoOfBedrooms,
                NoOfBathrooms = l.NoOfBathrooms,
                Sqft = l.Sqft,
                NoOfFloors = l.NoOfFloors,
                GarageSize = l.GarageSize,
                YearConstructed = l.YearConstructed,
                Description = l.Description,
                Email = l.Email,
                Phone = l.Phone,

                // Location
                Address = l.Address,
                City = l.City,
                State = l.State,
                ZipCode = l.ZipCode,



                
                Images = l.Images.Select(i => new ImageResponse { Path = i.Path }).ToList(),
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            }).ToList();
        }

        public async Task<ListingResponse> GetListingByIdAsync(Guid id)
        {
            var listing = await _listingRepository.GetListingByIdAsync(id);
            return new ListingResponse
            {
                Id = listing.Id,
                OwnerId = listing.UserId,
                PropertyName = listing.PropertyName,
                PropertyType = listing.PropertyType,
                CurrencyType = listing.CurrencyType,
                SalePrice = listing.SalePrice,
                OfferPrice = listing.OfferPrice,
                status = listing.status,
                // Property Details
                PropertyId = listing.PropertyId,
                PricePerSqft = listing.PricePerSqft,
                NoOfBedrooms = listing.NoOfBedrooms,
                NoOfBathrooms = listing.NoOfBathrooms,
                Sqft = listing.Sqft,
                NoOfFloors = listing.NoOfFloors,
                GarageSize = listing.GarageSize,
                YearConstructed = listing.YearConstructed,
                Description = listing.Description,
                Email = listing.Email,
                Phone = listing.Phone,

                  // Location
                Address = listing.Address,
                City = listing.City,
                State = listing.State,
                ZipCode = listing.ZipCode,
                Images = listing.Images.Select(i => new ImageResponse { Path = i.Path }).ToList(),
                 CreatedAt = listing.CreatedAt,
                 UpdatedAt = listing.UpdatedAt
            };
        }

        public async Task<List<ListingResponse>> GetListingsByUserIdAsync(Guid userId)
        {
            var listings = await _listingRepository.GetListingsByUserIdAsync(userId);
            return listings.Select(l => new ListingResponse
            {
                Id = l.Id,
                OwnerId = l.UserId,
                PropertyName = l.PropertyName,
                PropertyType = l.PropertyType,
                CurrencyType = l.CurrencyType,
                SalePrice = l.SalePrice,
                OfferPrice = l.OfferPrice,
                status = l.status,
                // Property Details
                PropertyId = l.PropertyId,
                PricePerSqft = l.PricePerSqft,
                NoOfBedrooms = l.NoOfBedrooms,
                NoOfBathrooms = l.NoOfBathrooms,
                Sqft = l.Sqft,
                NoOfFloors = l.NoOfFloors,
                GarageSize = l.GarageSize,
                YearConstructed = l.YearConstructed,
                Description = l.Description,
                Email = l.Email,
                Phone = l.Phone,

                // Location
                Address = l.Address,
                City = l.City,
                State = l.State,
                ZipCode = l.ZipCode,

               // Images

                 // Metadata
                 CreatedAt = l.CreatedAt,
                 UpdatedAt = l.UpdatedAt,
                 Images = l.Images.Select(i => new ImageResponse { Path = i.Path }).ToList(),

            }).ToList();
        }

        public async Task<ListingResponse> UpdateListingAsync(Guid id, UpdateListingRequest request)
        {
            try
            {
                // Fetch the existing listing from the repository
                var listing = await _listingRepository.GetListingByIdAsync(id);
                if (listing == null)
                    throw new Exception("Listing not found.");

                // Update the listing properties
                listing.PropertyName = request.PropertyName;
                listing.PropertyType = request.PropertyType;
                listing.CurrencyType = request.CurrencyType;
                listing.SalePrice = request.SalePrice;
                listing.OfferPrice = request.OfferPrice;
                listing.status = request.status;
                listing.PropertyId = request.PropertyId;
                listing.PricePerSqft = request.PricePerSqft;
                listing.NoOfBedrooms = request.NoOfBedrooms;
                listing.NoOfBathrooms = request.NoOfBathrooms;
                listing.Sqft = request.Sqft;
                listing.NoOfFloors = request.NoOfFloors;
                listing.GarageSize = request.GarageSize;
                listing.YearConstructed = request.YearConstructed;
                listing.Description = request.Description;
                listing.Email = request.Email;
                listing.Phone = request.Phone;
                listing.Address = request.Address;
                listing.City = request.City;
                listing.State = request.State;
                listing.ZipCode = request.ZipCode;
                listing.UpdatedAt = DateTime.UtcNow;

                // Save the changes to the database
                await _listingRepository.UpdateListingAsync(listing);

                // Return the updated listing as a response
                return new ListingResponse
                {
                    Id = listing.Id,
                    PropertyName = listing.PropertyName,
                    PropertyType = listing.PropertyType,
                    CurrencyType = listing.CurrencyType,
                    SalePrice = listing.SalePrice,
                    OfferPrice = listing.OfferPrice,
                    status = listing.status,
                    PropertyId = listing.PropertyId,
                    PricePerSqft = listing.PricePerSqft,
                    NoOfBedrooms = listing.NoOfBedrooms,
                    NoOfBathrooms = listing.NoOfBathrooms,
                    Sqft = listing.Sqft,
                    NoOfFloors = listing.NoOfFloors,
                    GarageSize = listing.GarageSize,
                    YearConstructed = listing.YearConstructed,
                    Description = listing.Description,
                    Email = listing.Email,
                    Phone = listing.Phone,
                    Address = listing.Address,
                    City = listing.City,
                    State = listing.State,
                    ZipCode = listing.ZipCode,
                    CreatedAt = listing.CreatedAt,
                    UpdatedAt = listing.UpdatedAt,
                    Images = listing.Images?.Select(i => new ImageResponse
                    {
                        Path = i.Path
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating listing: {ex.Message}");
                throw new Exception("An error occurred while updating the listing. Please try again.");
            }
        }

        public async Task<ListingResponse> ChangeListingStatusAsync(Guid id)
        {
            var listing = await _listingRepository.GetListingByIdAsync(id);
            if (listing == null) return null;

            var propertyType = listing.PropertyType?.ToLower();

            if (propertyType == "rent")
            {
                listing.status = listing.status == "available" ? "rented" : "available";
            }
            else if (propertyType == "buy")
            {
                listing.status = listing.status == "available" ? "not available" : "available";
            }

            var updatedListing = await _listingRepository.UpdateListingAsync(listing);

            return new ListingResponse
            {
                Id = updatedListing.Id,
                OwnerId = updatedListing.UserId,
                PropertyName = updatedListing.PropertyName,
                PropertyType = updatedListing.PropertyType,
                CurrencyType = updatedListing.CurrencyType,
                SalePrice = updatedListing.SalePrice,
                OfferPrice = updatedListing.OfferPrice,
                status = updatedListing.status,
                PropertyId = updatedListing.PropertyId,
                PricePerSqft = updatedListing.PricePerSqft,
                NoOfBedrooms = updatedListing.NoOfBedrooms,
                NoOfBathrooms = updatedListing.NoOfBathrooms,
                Sqft = updatedListing.Sqft,
                NoOfFloors = updatedListing.NoOfFloors,
                GarageSize = updatedListing.GarageSize,
                YearConstructed = updatedListing.YearConstructed,
                Description = updatedListing.Description,
                Email = updatedListing.Email,
                Phone = updatedListing.Phone,
                Address = updatedListing.Address,
                City = updatedListing.City,
                State = updatedListing.State,
                ZipCode = updatedListing.ZipCode,
                Images = updatedListing.Images.Select(i => new ImageResponse { Path = i.Path }).ToList(),
                CreatedAt = updatedListing.CreatedAt,
                UpdatedAt = updatedListing.UpdatedAt
            };
        }

        public async Task<ReviewResponse> AddReviewAsync(Guid listingId, ReviewRequest request)
        {
            var review = new Review
            {
                ListingId = listingId,
                UserId = request.UserId,
                Content = request.Content,
                Rating = request.Rating,
                Email= request.Email,
                name = request.name,
                CreatedAt = DateTime.UtcNow
            };

            var createdReview = await _listingRepository.AddReviewAsync(review);
            return new ReviewResponse
            {
                Id = createdReview.Id,
                ListingId = createdReview.ListingId,
                UserId = createdReview.UserId,
                Content = createdReview.Content,
                Rating = createdReview.Rating,
                Email = createdReview.Email,
                name = createdReview.name,
                CreatedAt = createdReview.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewsByListingIdAsync(Guid listingId)
        {
            var reviews = await _listingRepository.GetReviewsByListingIdAsync(listingId);
            return reviews.Select(r => new ReviewResponse
            {
                Id = r.Id,
                ListingId = r.ListingId,
                UserId = r.UserId,
                Content = r.Content,
                Rating = r.Rating,
                Email = r.Email,
                name = r.name,
                CreatedAt = r.CreatedAt
            });
        }

        public async Task<bool> DeleteReviewAsync(Guid listingId, Guid reviewId)
        {
            return await _listingRepository.DeleteReviewAsync(listingId, reviewId);
        }

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            return await _listingRepository.DeleteListingAsync(id);
        }

    }

}
