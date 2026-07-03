using EloisStore.Api.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(EloisStoreDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Categories.AnyAsync())
        {
            return;
        }

        var dresses = new Category { Name = "Vestidos", Slug = "vestidos" };
        var blouses = new Category { Name = "Blusas", Slug = "blusas" };

        dbContext.Categories.AddRange(dresses, blouses);

        dbContext.Products.Add(new Product
        {
            CategoryId = dresses.Id,
            Name = "Vestido Floral EloÍs",
            Description = "Vestido leve com estampa floral.",
            Price = 189.90m,
            Variants =
            [
                new ProductVariant { Sku = "VEST-FLORAL-P-ROSA", Size = "P", Color = "Rosa", StockQuantity = 12 },
                new ProductVariant { Sku = "VEST-FLORAL-M-ROSA", Size = "M", Color = "Rosa", StockQuantity = 8 }
            ]
        });

        await dbContext.SaveChangesAsync();
    }
}
