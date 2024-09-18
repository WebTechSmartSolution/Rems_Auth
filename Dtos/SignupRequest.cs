namespace Rems_Auth.Dtos
{
    public class SignupRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public  string? CountryCode { get; set; }
        public required string MobileNumber { get; set; }
        public bool? IsAgent { get; set; }
    }
}
