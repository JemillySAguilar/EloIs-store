using EloisStore.Api.Extensions;
using EloisStore.Api.Models.Cart;
using EloisStore.Api.Services.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/users/{userId:guid}/cart")]
public sealed class CartController(CartService carts) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Cart>> Get(Guid userId)
    {
        if (!User.CanAccessUser(userId)) return Forbid();
        return await carts.GetOrCreateAsync(userId);
    }

    [HttpPost("items")]
    public async Task<ActionResult<Cart>> AddItem(Guid userId, AddCartItemRequest request)
    {
        if (!User.CanAccessUser(userId)) return Forbid();
        return await carts.AddItemAsync(userId, request);
    }
}