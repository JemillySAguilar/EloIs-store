using EloisStore.Api.Models.Orders;

namespace EloisStore.Api.Services.Notifications;

public sealed class NotificationService(EmailService emails)
{
    public Task NotifyOrderConfirmedAsync(Order order) =>
        emails.SendAsync("customer@example.com", "Pedido confirmado", $"Pedido {order.Id} confirmado na EloÍs Store.");
}
