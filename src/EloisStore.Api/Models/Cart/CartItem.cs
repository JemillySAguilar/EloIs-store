namespace EloisStore.Api.Models.Cart;

public sealed class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
