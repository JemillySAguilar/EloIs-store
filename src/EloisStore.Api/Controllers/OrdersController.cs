using EloisStore.Api.Extensions;
using EloisStore.Api.Models.Orders;
using EloisStore.Api.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public sealed class OrdersController(OrderService orders, CheckoutService checkout) : ControllerBase
{
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<List<Order>>> ListByUser(Guid userId)
    {
        if (!User.CanAccessUser(userId)) return Forbid();
        return await orders.ListByUserIdAsync(userId);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Order>> Find(Guid id)
    {
        var order = await orders.FindAsync(id);
        if (order is null) return NotFound();
        if (!User.CanAccessUser(order.UserId)) return Forbid();
        return order;
    }

    [HttpPost("checkout")]
    public Task<Order> Checkout(CheckoutRequest request) =>
        checkout.CheckoutAsync(User.GetUserId(), request.PaymentMethod);
}