using EloisStore.Api.Models.Cart;
using EloisStore.Api.Models.Cart;
using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Models.Orders;
using EloisStore.Api.Models.Payments;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Data;

public sealed class EloisStoreDbContext(DbContextOptions<EloisStoreDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> CategoriaDoBanco => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(category => category.Slug).IsUnique();
        modelBuilder.Entity<Product>().HasMany(product => product.Variants).WithOne().HasForeignKey(variant => variant.ProductId);
        modelBuilder.Entity<Cart>().HasMany(cart => cart.Items).WithOne().HasForeignKey(item => item.CartId);
        modelBuilder.Entity<Order>().HasMany(order => order.Items).WithOne().HasForeignKey(item => item.OrderId);
    }
}
