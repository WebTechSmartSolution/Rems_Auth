using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Rems_Auth.Utilities
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Return the salt and the hashed password in a "{salt}:{hash}" format
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Split the stored hash into the salt and the hash itself
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false; // Invalid format
            }

            var salt = Convert.FromBase64String(parts[0]);

            // Re-hash the provided password using the extracted salt
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Compare the newly hashed password with the stored hash
            return hash == parts[1];
        }
    }
}
