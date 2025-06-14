using Microsoft.AspNetCore.Mvc;
using Rems_Auth.Dtos;
using Rems_Auth.Repositories;
using Rems_Auth.Services;
using System.Security.Claims;

namespace Rems_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly IUserService _userService;
        private readonly IListingRepository _listingRepository;

        public ListingsController(IListingService listingService, IUserService userService , IListingRepository listingRepository)
        {
            _userService = userService;
            _listingService = listingService;
            _listingRepository = listingRepository;
        }

       
        [HttpPost("AddListings")]

        public async Task<ActionResult<ListingResponse>> AddListing([FromForm] ListingRequest request)
        {
            try
            {
                // Extract user ID from the token using IHttpContextAccessor
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //var userId = User.FindFirst("nameid")?.Value;
                if (!Guid.TryParse(userId, out var parsedUserId))
                {
                    return Unauthorized("Invalid user ID in token.");
                }

                // Call the service to create the listing
                var listingResponse = await _listingService.CreateListingAsync(request, parsedUserId);

                // Return CreatedAtAction response
                return CreatedAtAction(nameof(GetListingById), new { id = listingResponse.Id }, listingResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<ListingResponse>>> GetAllListings()
        {
            try
            {
                var listings = await _listingService.GetAllListingsAsync();
                return Ok(listings);
            }
            catch (Exception ex)
            {
                // Log the exception (Console, Serilog, or another logging framework)
                Console.WriteLine($"Error fetching listings: {ex.Message}");

                // Return a generic error message to avoid exposing sensitive details
                return StatusCode(500, "An unexpected error occurred while retrieving listings.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ListingResponse>> GetListingById(Guid id)
        {
            var listing = await _listingService.GetListingByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }
            return Ok(listing);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ListingResponse>>> GetListingsByUserId(Guid userId)
        {
            var listings = await _listingService.GetListingsByUserIdAsync(userId);
            return Ok(listings);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingRequest request)
        {
            var updatedListing = await _listingService.UpdateListingAsync(id, request);
            if (updatedListing == null)
            {
                return NotFound();
            }
            return Ok(updatedListing);
        }

        [HttpPatch("{id}/ChangeStatus")]
        public async Task<IActionResult> ChangeListingStatus(Guid id)
        {
            try
            {
                // Call the service to toggle the status
                var updatedListing = await _listingService.ChangeListingStatusAsync(id);
                if (updatedListing == null)
                {
                    return NotFound("Listing not found.");
                }
                return Ok(updatedListing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("reviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _listingRepository.GetAllReviewsAsync();
                if (reviews == null || !reviews.Any())
                {
                    return NotFound("No reviews found.");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviewsByListingId(Guid id)
        {
            try
            {
                var reviews = await _listingService.GetReviewsByListingIdAsync(id);
                if (reviews == null || !reviews.Any())
                {
                    return NotFound("No reviews found for this listing.");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> AddReview(Guid id, [FromBody] ReviewRequest request)
        {
            try
            {
                var review = await _listingService.AddReviewAsync(id, request);
                return CreatedAtAction(nameof(GetListingById), new { id = review.ListingId }, review);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}/reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid id, Guid reviewId)
        {
            try
            {
                var deleted = await _listingService.DeleteReviewAsync(id, reviewId);
                if (!deleted)
                {
                    return NotFound("Review not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteListing(Guid id)
        {
            var success = await _listingService.DeleteListingAsync(id);
            if (!success)
            {
                return NotFound("Listing not found.");
            }
            return Ok("Listing Deleted Sucessfully");
        }


        [HttpGet("dashboard-stats")]
        public async Task<ActionResult<DashboardStatsResponse>> GetDashboardStats()
        {
            try
            {
                // Get total listings (You can customize this based on your requirements)
                var totalListings = await _listingService.GetAllListingsAsync();
                var totalListingsCount = totalListings.Count;

                // Get total users (You might need to implement a GetTotalUsersAsync method in your service)
                var totalUsers = await _userService.GetTotalUsersAsync(); // Add this method in your IListingService if not present

                // Get total sold listings
                var totalSoldListings = totalListings.Count(listing =>
     !string.IsNullOrWhiteSpace(listing.status) &&
     listing.status.Trim().Equals("Not available", StringComparison.OrdinalIgnoreCase));

                var totalRevenue = totalListings
                    .Where(listing => !string.IsNullOrWhiteSpace(listing.status) &&
                                      listing.status.Trim().Equals("Not available", StringComparison.OrdinalIgnoreCase))
                    .Sum(listing => listing.SalePrice);
                // Use the appropriate price field
                Console.WriteLine(totalRevenue);

                // Constructing the response
                var dashboardStats = new DashboardStatsResponse
                {
                    TotalListings = totalListingsCount,
                    TotalUsers = totalUsers,
                    TotalSoldListings = totalSoldListings,
                    TotalRevenue = totalRevenue
                };

                return Ok(dashboardStats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }

}
