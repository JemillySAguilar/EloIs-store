using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Catalog;

public sealed class ProductService(ProductRepository products)
{
    public Task<List<Product>> ListAsync() => products.ListAsync();

    public Task<Product?> FindAsync(Guid id) => products.FindAsync(id);

    public Task<Product> CreateAsync(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Price <= 0)
            throw new InvalidOperationException("Nome e valor válido são obrigatórios.");
        if (request.Variants.Count == 0)
            throw new InvalidOperationException("Cadastre ao menos uma variação do produto.");

        var product = new Product
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
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
