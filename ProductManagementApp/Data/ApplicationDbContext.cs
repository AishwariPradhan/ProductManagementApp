using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Models;

namespace ProductManagementApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<allProduct> Products { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Locale> Locales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<allProduct>().ToTable("allProduct", schema: "product");
            modelBuilder.Entity<Price>().ToTable("Price", schema: "product");
            modelBuilder.Entity<Locale>().ToTable("Locale", schema: "common");

            base.OnModelCreating(modelBuilder);
        }
    }
}
