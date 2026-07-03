using EloisStore.Api.Models.Catalog;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Catalog;

public sealed class ProductSearchService(ProductRepository products)
{
    public Task<List<Product>> SearchAsync(string term) =>
        string.IsNullOrWhiteSpace(term) ? products.ListAsync() : products.SearchAsync(term);
}
