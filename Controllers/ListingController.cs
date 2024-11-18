using Microsoft.AspNetCore.Mvc;
using Rems_Auth.Dtos;
using Rems_Auth.Services;

namespace Rems_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _service;

        public ListingsController(IListingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddListing(ListingRequest request)
        {
            try
            {
                var response = await _service.AddListingAsync(request);
                return CreatedAtAction(nameof(GetListingById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                // Log the exception (use logging library in production)
                Console.WriteLine($"Error in AddListing: {ex.Message}");
                return StatusCode(500, "An error occurred while adding the listing.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllListings()
        {
            try
            {
                var listings = await _service.GetAllListingsAsync();
                return Ok(listings);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllListings: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving listings.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetListingById(int id)
        {
            try
            {
                var listing = await _service.GetListingByIdAsync(id);
                if (listing == null) return NotFound();
                return Ok(listing);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetListingById: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the listing.");
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetListingsByUser(int userId)
        {
            try
            {
                var listings = await _service.GetListingsByUserAsync(userId);
                if (listings == null || !listings.Any()) return NotFound($"No listings found for user with ID {userId}.");
                return Ok(listings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetListingsByUser: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving user-specific listings.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListing(int id)
        {
            try
            {
                var result = await _service.DeleteListingAsync(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in DeleteListing: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the listing.");
            }
        }
    }
}
