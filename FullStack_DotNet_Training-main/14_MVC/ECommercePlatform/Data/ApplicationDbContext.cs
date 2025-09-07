using Microsoft.EntityFrameworkCore;
using ECommercePlatform.Models;

namespace ECommercePlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            // Seed data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "Gaming Laptop", Price = 999.99m, Stock = 10 },
                new Product { Id = 2, Name = "Mouse", Description = "Wireless Mouse", Price = 29.99m, Stock = 50 },
                new Product { Id = 3, Name = "Keyboard", Description = "Mechanical Keyboard", Price = 79.99m, Stock = 25 }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Email = "admin@test.com", Password = "admin123", Role = "Admin" },
                new User { Id = 2, Username = "user", Email = "user@test.com", Password = "user123", Role = "User" }
            );
        }
    }
}