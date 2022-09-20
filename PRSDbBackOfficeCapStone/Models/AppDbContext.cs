using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace PRSDbBackOfficeCapStone.Models
{
    public class AppDbContext : DbContext
    {

        //Database Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }





        public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options) { }
        protected override void OnModelCreating(ModelBuilder builder) { }

    }
}
