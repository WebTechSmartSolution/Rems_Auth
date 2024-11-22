using Microsoft.AspNetCore.Mvc;
using Rems_Auth.Dtos;
using Rems_Auth.Services;
using System.Security.Claims;

namespace Rems_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;

        public ListingsController(IListingService listingService)
        {
            _listingService = listingService;
        }

        [HttpPost]
        // File: Controllers/ListingController.cs
        [HttpPost]
        public async Task<ActionResult<ListingResponse>> AddListing([FromForm] ListingRequest request)
        {
            try
            {
                // Extract user ID from the token using IHttpContextAccessor
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
            return Ok(await _listingService.GetAllListingsAsync());
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingRequest request)
        {
            var updatedListing = await _listingService.UpdateListingAsync(id, request);
            if (updatedListing == null)
            {
                return NotFound();
            }
            return Ok(updatedListing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListing(Guid id)
        {
            var success = await _listingService.DeleteListingAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
