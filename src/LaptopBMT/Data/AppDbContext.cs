using Microsoft.EntityFrameworkCore;
using LaptopBMT.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LaptopBMT.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Các DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ giữa User và Cart (1 - nhiều)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)               // ✅ liên kết tới entity User
                .WithMany(u => u.Carts)            // ✅ 1 user có nhiều carts
                .HasForeignKey(c => c.UserId)      // ✅ khóa ngoại
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // Quan hệ giữa Cart và CartItem (1 - nhiều)
            // ===============================
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // Quan hệ giữa CartItem và Product (1 - nhiều)
            // ===============================
            
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // Quan hệ giữa Order và OrderDetail (1 - nhiều)
            // ===============================
            modelBuilder.Entity<Order>()
                .WithOne(od => od.Order)
                .HasMany<OrderDetail>(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // Quan hệ giữa OrderDetail và Product (1 - nhiều)
            // ===============================
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
