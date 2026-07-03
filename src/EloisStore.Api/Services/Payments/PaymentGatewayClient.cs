using EloisStore.Api.Models.Payments;
using Microsoft.Extensions.Options;
using EloisStore.Api.Configurations;

namespace EloisStore.Api.Services.Payments;

public sealed class PaymentGatewayClient(IOptions<PaymentGatewaySettings> settings)
{
    public Task<PaymentStatus> ChargeAsync(decimal amount, string method)
    {
        var approved = settings.Value.ForceApproval && amount > 0 && !string.IsNullOrWhiteSpace(method);
        return Task.FromResult(approved ? PaymentStatus.Approved : PaymentStatus.Refused);
    }
}
