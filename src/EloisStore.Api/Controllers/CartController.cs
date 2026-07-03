using EloisStore.Api.Models.Cart;
using EloisStore.Api.Services.Cart;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Route("api/users/{userId:guid}/cart")]
public sealed class CartController(CartService carts) : ControllerBase
{
    [HttpGet]
    public Task<Cart> Get(Guid userId) => carts.GetOrCreateAsync(userId);

    [HttpPost("items")]
    public Task<Cart> AddItem(Guid userId, AddCartItemRequest request) => carts.AddItemAsync(userId, request);
}
