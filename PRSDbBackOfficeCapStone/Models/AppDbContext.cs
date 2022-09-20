using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace PRSDbBackOfficeCapStone.Models
{
    public class AppDbContext : DbContext
    {

        //Database Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestLine> RequestLines { get; set; }




        public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options) { }
        protected override void OnModelCreating(ModelBuilder builder) { }

    }
}
