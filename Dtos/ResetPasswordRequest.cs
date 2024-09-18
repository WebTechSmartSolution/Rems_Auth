namespace Rems_Auth.Dtos
{
    public class ResetPasswordRequest
    {
        public required string NewPassword { get; set; }
        public required string Token { get; set; }
    }
}
