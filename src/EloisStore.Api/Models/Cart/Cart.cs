namespace EloisStore.Api.Models.Cart;

public sealed class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = [];
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
