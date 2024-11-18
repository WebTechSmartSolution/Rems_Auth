using Rems_Auth.Dtos;
using Rems_Auth.Models;
using Rems_Auth.Repositories;

namespace Rems_Auth.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _repository;

        public ListingService(IListingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ListingResponse> AddListingAsync(ListingRequest request)
        {
            var newListing = new AddListing
            {
                PropertyName = request.PropertyName,
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
                Title = request.Title,
                TotalArea = request.TotalArea,
                Description = request.Description,
                Bedrooms = request.Bedrooms,
                Bathrooms = request.Bathrooms,
                GarageSizeDescription = request.GarageSizeDescription,
                YearBuilt = request.YearBuilt,
                Address = request.Address,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                Images = request.Images,
                Document = request.Document,
                UserId = request.UserId
            };

            var createdListing = await _repository.AddListingAsync(newListing);
            return new ListingResponse
            {
                Id = createdListing.Id,
                PropertyName = createdListing.PropertyName,
                SalePrice = createdListing.SalePrice,
                OfferPrice = createdListing.OfferPrice,
                Address = createdListing.Address,
                City = createdListing.City,
                State = createdListing.State,
                ZipCode = createdListing.ZipCode,
                CreatedAt = createdListing.CreatedAt
            };
        }
        public async Task<IEnumerable<ListingResponse>> GetListingsByUserAsync(int userId)
        {
            var listings = await _repository.GetListingsByUserAsync(userId);

            return listings.Select(listing => new ListingResponse
            {
                Id = listing.Id,
                PropertyName = listing.PropertyName,
                SalePrice = listing.SalePrice,
                OfferPrice = listing.OfferPrice,
                Address = listing.Address,
                City = listing.City,
                State = listing.State,
                ZipCode = listing.ZipCode,
                CreatedAt = listing.CreatedAt,
                UpdatedAt = listing.UpdatedAt
                // Map other fields as needed
            });
        }



        public async Task<IEnumerable<ListingResponse>> GetAllListingsAsync()
        {
            var listings = await _repository.GetAllListingsAsync();
            return listings.Select(listing => new ListingResponse
            {
                Id = listing.Id,
                PropertyName = listing.PropertyName,
                SalePrice = listing.SalePrice,
                OfferPrice = listing.OfferPrice,
                Address = listing.Address,
                City = listing.City,
                State = listing.State,
                ZipCode = listing.ZipCode,
                CreatedAt = listing.CreatedAt
            });
        }

        public async Task<ListingResponse?> GetListingByIdAsync(int id)
        {
            var listing = await _repository.GetListingByIdAsync(id);
            if (listing == null) return null;

            return new ListingResponse
            {
                Id = listing.Id,
                PropertyName = listing.PropertyName,
                SalePrice = listing.SalePrice,
                OfferPrice = listing.OfferPrice,
                Address = listing.Address,
                City = listing.City,
                State = listing.State,
                ZipCode = listing.ZipCode,
                CreatedAt = listing.CreatedAt
            };
        }

        public async Task<bool> DeleteListingAsync(int id)
        {
            return await _repository.DeleteListingAsync(id);
        }

       
    }
}
