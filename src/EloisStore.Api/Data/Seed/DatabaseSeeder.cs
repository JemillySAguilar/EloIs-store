using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Models.Cart;
using EloisStore.Api.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(EloisStoreDbContext dbContext, PasswordService passwords, IConfiguration configuration)
    {
        await dbContext.Database.EnsureCreatedAsync();

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "Phone" text NOT NULL DEFAULT '';
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "Street" text NOT NULL DEFAULT '';
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "Number" text NOT NULL DEFAULT '';
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "Complement" text NOT NULL DEFAULT '';
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "Neighborhood" text NOT NULL DEFAULT '';
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "City" text NOT NULL DEFAULT '';
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "State" text NOT NULL DEFAULT '';
            ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "PostalCode" text NOT NULL DEFAULT '';
            """);

        var adminEmail = configuration["AdminUser:Email"]?.Trim().ToLowerInvariant();
        var adminPassword = configuration["AdminUser:Password"];
        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var admin = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == adminEmail);
            if (admin is null)
            {
                dbContext.Users.Add(new User
                {
                    Name = configuration["AdminUser:Name"] ?? "Administradora Elois",
                    Email = adminEmail,
                    PasswordHash = passwords.Hash(adminPassword),
                    Role = "Admin"
                });
            }
            else if (admin.Role != "Admin")
            {
                admin.Role = "Admin";
            }
        }

        if (!await dbContext.Categories.AnyAsync())
        {
        var dresses = new Category { Name = "Vestidos", Slug = "vestidos" };
        var blouses = new Category { Name = "Blusas", Slug = "blusas" };
        var sets = new Category { Name = "Conjuntos", Slug = "conjuntos" };

        dbContext.Categories.AddRange(dresses, blouses, sets);

        dbContext.Products.AddRange(
            new Product
            {
                CategoryId = dresses.Id,
                Name = "Vestido Floral Elois",
                Description = "Vestido leve com estampa floral e caimento fluido.",
                Price = 189.90m,
                ImageUrl = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80",
                Variants =
                [
                    new ProductVariant { Sku = "VEST-FLORAL-P-ROSA", Size = "P", Color = "Rosa", StockQuantity = 12 },
                    new ProductVariant { Sku = "VEST-FLORAL-M-ROSA", Size = "M", Color = "Rosa", StockQuantity = 8 }
                ]
            },
            new Product
            {
                CategoryId = blouses.Id,
                Name = "Blusa Luna Off White",
                Description = "Blusa versatil em tecido macio para compor looks delicados.",
                Price = 119.90m,
                ImageUrl = "https://images.unsplash.com/photo-1554568218-0f1715e72254?auto=format&fit=crop&w=900&q=80",
                Variants =
                [
                    new ProductVariant { Sku = "BLU-LUNA-P-OFF", Size = "P", Color = "Off white", StockQuantity = 10 },
                    new ProductVariant { Sku = "BLU-LUNA-G-OFF", Size = "G", Color = "Off white", StockQuantity = 6 }
                ]
            },
            new Product
            {
                CategoryId = sets.Id,
                Name = "Conjunto Siena Alfaiataria",
                Description = "Conjunto moderno com acabamento elegante para o dia inteiro.",
                Price = 249.90m,
                ImageUrl = "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=900&q=80",
                Variants =
                [
                    new ProductVariant { Sku = "CONJ-SIENA-M-BEGE", Size = "M", Color = "Bege", StockQuantity = 7 },
                    new ProductVariant { Sku = "CONJ-SIENA-G-BEGE", Size = "G", Color = "Bege", StockQuantity = 5 }
                ]
            });
        }

        await dbContext.SaveChangesAsync();
    }
}
