using Microsoft.EntityFrameworkCore;
using Rems_Auth.Models;
using System.Collections.Generic;
using System.Reflection;

namespace Rems_Auth.Data
{
   
      public class ApplicationDbContext : DbContext
      {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

        public DbSet<AddListing> Listings { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
        }

    }

}
