using Microsoft.AspNetCore.Authorization;
using EloisStore.Api.Models.Orders;
using EloisStore.Api.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public sealed class OrdersController(OrderService orders, CheckoutService checkout) : ControllerBase
{
    [HttpGet("user/{userId:guid}")]
    public Task<List<Order>> ListByUser(Guid userId) => orders.ListByUserIdAsync(userId);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Order>> Find(Guid id)
    {
        var order = await orders.FindAsync(id);
        return order is null ? NotFound() : order;
    }

    [HttpPost("checkout")]
    public Task<Order> Checkout(CheckoutRequest request) => checkout.CheckoutAsync(request);
}
