using EloisStore.Api.Models.Orders;
using EloisStore.Api.Services.Cart;
using EloisStore.Api.Services.Notifications;
using EloisStore.Api.Services.Payments;

namespace EloisStore.Api.Services.Orders;

public sealed class CheckoutService(
    CartService carts,
    OrderService orders,
    PaymentService payments,
    NotificationService notifications)
{
    public async Task<Order> CheckoutAsync(Guid userId, string paymentMethod)
    {
        var cart = await carts.GetOrCreateAsync(userId);
        if (cart.Items.Count == 0) throw new InvalidOperationException("Cart is empty.");

        var order = await orders.CreatePendingAsync(cart);
        var payment = await payments.PayAsync(order.Id, order.TotalAmount, paymentMethod);
        order.Status = payment.Status == EloisStore.Api.Models.Payments.PaymentStatus.Approved
            ? OrderStatus.Confirmed
            : OrderStatus.Cancelled;

        await orders.SaveAsync(order);
        if (order.Status == OrderStatus.Confirmed) await notifications.NotifyOrderConfirmedAsync(order);
        return order;
    }
}