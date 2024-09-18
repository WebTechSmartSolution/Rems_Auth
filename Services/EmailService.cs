using Microsoft.Extensions.Options;
using Rems_Auth.Utilities;
using System.Net.Mail;
using System.Net;

namespace Rems_Auth.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        // Using IOptionsMonitor to get the current configuration value
        public EmailService(IOptionsMonitor<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.CurrentValue;
        }

        public async Task SendPasswordResetEmailAsync(string email, string token, string resetLink)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                {
                    Port = _emailSettings.SmtpPort,
                    Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail),
                    Subject = "Password Reset Request",
                    Body = $@"
                <html>
                <body>
                    <p>Please reset your password using the following link:</p>
                    <p><a href=""{resetLink}"" target=""_blank"">Reset Password</a></p>
                    <p>If the link doesn't work, copy and paste this URL into your browser:</p>
                    <p>{resetLink}</p>
                    <p>If you were not expecting this, please disregard this email.</p>
                    <p>Also, you can use the following token to reset your password: <strong>{token}</strong></p>
                </body>
                </html>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the error or throw it
                throw new Exception("Email sending failed", ex);
            }
        }

    }
}
