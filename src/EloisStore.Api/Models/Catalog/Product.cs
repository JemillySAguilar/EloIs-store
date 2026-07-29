namespace EloisStore.Api.Models.Catalog;

public sealed class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public List<ProductVariant> Variants { get; set; } = [];
}
