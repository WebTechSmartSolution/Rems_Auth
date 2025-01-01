using Microsoft.EntityFrameworkCore;
using Rems_Auth.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Rems_Auth.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<AddListing> Listings { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedAdmin(builder);
            // Configure the relationship between Listing and User
            builder.Entity<AddListing>()
                .HasOne(l => l.User)
                .WithMany()  // A user can have many listings
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between Listing and Image
            builder.Entity<Image>()
                .HasOne(i => i.Listing)
                .WithMany(l => l.Images)
                .HasForeignKey(i => i.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
               .HasOne(m => m.Sender)
               .WithMany(u => u.SentMessages)
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public void SeedAdmin(ModelBuilder modelBuilder)
        {
            var defaultAdmin = new Admin
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = HashPassword("Admin@123"),
                CreatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<Admin>().HasData(defaultAdmin);
        }
        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }

    }

}
