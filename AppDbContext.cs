using System.Reflection.Emit;
using MarketAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(e => e.StockQuantity)
                .IsRequired()
                .HasDefaultValue(20);
        }



    }


}
