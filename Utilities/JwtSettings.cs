namespace Rems_Auth.Utilities
{
    public class JwtSettings
    {
        public required string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
        
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
