using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Catalog;

public sealed class ProductService(ProductRepository products)
{
    public Task<List<Product>> ListAsync() => products.ListAsync();

    public Task<Product?> FindAsync(Guid id) => products.FindAsync(id);

    public Task<Product> CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Variants = request.Variants.Select(variant => new ProductVariant
            {
                Sku = variant.Sku,
                Size = variant.Size,
                Color = variant.Color,
                StockQuantity = variant.StockQuantity
            }).ToList()
        };

        return products.AddAsync(product);
    }
}
