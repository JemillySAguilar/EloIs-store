namespace EloisStore.Api.Models.Catalog;

public sealed record ProductResponse(
    Guid Id,
    Guid CategoryId,
    string Name,
    string Description,
    decimal Price,
    bool IsActive,
    IReadOnlyCollection<ProductVariant> Variants);
