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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships between User and Listing
            modelBuilder.Entity<AddListing>()
                .HasOne(listing => listing.User)
                .WithMany(user => user.Listings)
                .HasForeignKey(listing => listing.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete listings when user is deleted

            base.OnModelCreating(modelBuilder);
        }
    }

}
