namespace Rems_Auth.Dtos
{
    public class TokenRequest
    {
        public  string AccessToken { get; set; }  // Optional, if you want to validate the current (expired) token
        public string RefreshToken { get; set; }
    }
}
