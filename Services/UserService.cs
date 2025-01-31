using Rems_Auth.Models;
using Rems_Auth.Repositories;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Rems_Auth.Dtos;
using Microsoft.AspNetCore.Identity;
using Rems_Auth.Utilities;
using Microsoft.AspNet.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rems_Auth.Data;

namespace Rems_Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IListingRepository _listingRepository;
        private readonly ApplicationDbContext _context;


        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,IListingRepository listingRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
            _context = context ?? throw new ArgumentNullException(nameof(_context));
        }


        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                CountryCode = user.CountryCode,
                ProfileImageUrl = user.ProfilePictureUrl,
                IsAgent = user.IsAgent,
                TotalListings = user.Listings.Count,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            });
        }

        //public async Task DeleteUserAndListingsAsync(Guid userId)
        //{
        //    var user = await _userRepository.GetUserByIdAsync(userId);
        //    if (user == null)
        //    {
        //        throw new Exception("User not found");
        //    }

        //    // Fetch and delete related listings
        //    var listings = await _listingRepository.GetListingsByUserIdAsync(userId);
        //    foreach (var listing in listings)
        //    {
        //        await _listingRepository.DeleteListingAsync(listing);
        //    }

        //    // Delete the user
        //    await _userRepository.DeleteUserAsync(user);
        //}
        public async Task<User> UpdateUserAsync(Guid userId, UserUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            if (!string.IsNullOrEmpty(request.CurrentPassword) && !string.IsNullOrEmpty(request.NewPassword))
            {
                if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
                    throw new Exception("Current password is incorrect.");

                user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            }

            if (request.ProfileImage != null)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.ProfileImage.FileName)}";
                var filePath = Path.Combine("wwwroot/images/profiles", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ProfileImage.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = $"/images/profiles/{fileName}";
            }

            user.Name = request.Name;
            user.MobileNumber = request.MobileNumber;
            user.CountryCode = request.CountryCode;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword)
                == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
        }


        public async Task UpdateUserProfilePictureAsync(Guid userId, IFormFile profilePicture)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profilePicture.FileName)}";
            var filePath = Path.Combine("wwwroot/images/profiles", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            var profilePictureUrl = $"/images/profiles/{fileName}";
            await _userRepository.UpdateUserProfilePictureAsync(userId, profilePictureUrl);
        }

        // Helper methods for password hashing
        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Extract the hash and verify it matches
            string hashedInput = HashPassword(password);
            return hashedInput == storedHash;
        }
        public async Task<int> GetTotalUsersAsync()
        {
            // Assuming _userRepository is injected and provides access to the user database table
            var users = await _userRepository.GetAllUsersAsync();
            return users.Count();
        }
        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            try
            {
                // Load the user with all associated data, including images
                var user = await _context.Users
                    .Include(u => u.Listings)
                        .ThenInclude(l => l.Images)  // Ensure images are included
                    .Include(u => u.Listings)
                        .ThenInclude(l => l.Chats)
                        .ThenInclude(c => c.Messages)
                    .Include(u => u.SentMessages)
                    .Include(u => u.ReceivedMessages)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    // Delete listings and their associated data
                    foreach (var listing in user.Listings)
                    {
                        foreach (var chat in listing.Chats)
                        {
                            _context.Messages.RemoveRange(chat.Messages);  // Remove all messages in the chat
                            _context.Chats.Remove(chat);  // Remove the chat itself
                        }

                        // Check if Images is null or empty before removing
                        if (listing.Images != null && listing.Images.Any())
                        {
                            _context.Images.RemoveRange(listing.Images);  // Remove all images associated with the listing
                        }

                        _context.Listings.Remove(listing);  // Remove the listing itself
                    }

                    // Delete all messages associated with the user
                    _context.Messages.RemoveRange(user.SentMessages);
                    _context.Messages.RemoveRange(user.ReceivedMessages);

                    // Remove the user
                    _context.Users.Remove(user);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;  // User not found
            }
            catch (Exception ex)
            {
                // Log the exception (you can replace this with your logger)
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;  // Return false if an error occurs
            }
        }

    }
}
