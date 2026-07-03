namespace EloisStore.Api.Models.Catalog;

public sealed record CreateProductRequest(
    Guid CategoryId,
    string Name,
    string Description,
    decimal Price,
    IReadOnlyCollection<CreateProductVariantRequest> Variants);

public sealed record CreateProductVariantRequest(string Sku, string Size, string Color, int StockQuantity);
