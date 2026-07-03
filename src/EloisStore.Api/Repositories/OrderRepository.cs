using EloisStore.Api.Data;
using EloisStore.Api.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class OrderRepository(EloisStoreDbContext dbContext)
{
    public Task<List<Order>> ListByUserIdAsync(Guid userId) =>
        dbContext.Orders.Include(order => order.Items).Where(order => order.UserId == userId).ToListAsync();

    public Task<Order?> FindAsync(Guid id) =>
        dbContext.Orders.Include(order => order.Items).FirstOrDefaultAsync(order => order.Id == id);

    public async Task<Order> SaveAsync(Order order)
    {
        if (dbContext.Entry(order).State == EntityState.Detached)
        {
            dbContext.Orders.Add(order);
        }

        await dbContext.SaveChangesAsync();
        return order;
    }
}
