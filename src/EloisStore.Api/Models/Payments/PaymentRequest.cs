namespace EloisStore.Api.Models.Payments;

public sealed record PaymentRequest(Guid OrderId, decimal Amount, string Method);
