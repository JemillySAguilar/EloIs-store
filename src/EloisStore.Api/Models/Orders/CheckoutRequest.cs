namespace EloisStore.Api.Models.Orders;

public sealed record CheckoutRequest(Guid UserId, string PaymentMethod);
