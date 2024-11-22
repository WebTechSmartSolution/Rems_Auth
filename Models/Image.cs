

using Rems_Auth.Models;

public class Image
{
    public Guid Id { get; set; }
    public string Path { get; set; }
    public Guid ListingId { get; set; }
    public AddListing Listing { get; set; }
}
