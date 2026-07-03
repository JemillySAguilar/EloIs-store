using EloisStore.Api.Models.Payments;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Payments;

public sealed class PaymentService(PaymentRepository payments, PaymentGatewayClient gateway)
{
    public Task<List<Payment>> ListByOrderIdAsync(Guid orderId) => payments.ListByOrderIdAsync(orderId);

    public async Task<Payment> PayAsync(Guid orderId, decimal amount, string method)
    {
        var status = await gateway.ChargeAsync(amount, method);
        return await payments.AddAsync(new Payment
        {
            OrderId = orderId,
            Amount = amount,
            Method = method,
            Status = status
        });
    }
}
