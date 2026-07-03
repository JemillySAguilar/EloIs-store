using EloisStore.Api.Data;
using EloisStore.Api.Models.Payments;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class PaymentRepository(EloisStoreDbContext dbContext)
{
    public Task<List<Payment>> ListByOrderIdAsync(Guid orderId) =>
        dbContext.Payments.Where(payment => payment.OrderId == orderId).ToListAsync();

    public async Task<Payment> AddAsync(Payment payment)
    {
        dbContext.Payments.Add(payment);
        await dbContext.SaveChangesAsync();
        return payment;
    }
}
