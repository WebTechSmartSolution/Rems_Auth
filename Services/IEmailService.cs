namespace Rems_Auth.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string token, string resetLink);
    }

}
