using EloisStore.Api.Models.Orders;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Orders;

public sealed class OrderService(OrderRepository orders)
{
    public Task<List<Order>> ListByUserIdAsync(Guid userId) => orders.ListByUserIdAsync(userId);

    public Task<Order?> FindAsync(Guid id) => orders.FindAsync(id);

    public Task<Order> CreatePendingAsync(EloisStore.Api.Models.Cart.Cart cart)
    {
        var order = new Order
        {
            UserId = cart.UserId,
            Status = OrderStatus.Pending,
            Items = cart.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductVariantId = item.ProductVariantId,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            }).ToList()
        };

        order.TotalAmount = order.Items.Sum(item => item.UnitPrice * item.Quantity);
        return orders.SaveAsync(order);
    }

    public Task<Order> SaveAsync(Order order) => orders.SaveAsync(order);
}
