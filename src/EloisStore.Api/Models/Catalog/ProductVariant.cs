namespace EloisStore.Api.Models.Catalog;

public sealed class ProductVariant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
}
