using Microsoft.EntityFrameworkCore;
using Rems_Auth.Models;
using System.Collections.Generic;

namespace Rems_Auth.Data
{
   
      public class ApplicationDbContext : DbContext
      {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    // Override OnModelCreating if necessary
      }

}
