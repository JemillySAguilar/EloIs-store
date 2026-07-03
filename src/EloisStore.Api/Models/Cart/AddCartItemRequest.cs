namespace EloisStore.Api.Models.Cart;

public sealed record AddCartItemRequest(Guid ProductId, Guid ProductVariantId, int Quantity);
