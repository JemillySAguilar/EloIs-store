using EloisStore.Api.Models.Cart;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Cart;

public sealed class CartService(CartRepository carts, ProductRepository products)
{
    public async Task<EloisStore.Api.Models.Cart.Cart> GetOrCreateAsync(Guid userId)
    {
        var cart = await carts.FindByUserIdAsync(userId);
        if (cart is not null)
        {
            return cart;
        }

        return await carts.SaveAsync(new EloisStore.Api.Models.Cart.Cart { UserId = userId });
    }

    public async Task<EloisStore.Api.Models.Cart.Cart> AddItemAsync(Guid userId, AddCartItemRequest request)
    {
        var product = await products.FindAsync(request.ProductId)
            ?? throw new InvalidOperationException("Product not found.");

        var variant = product.Variants.FirstOrDefault(item => item.Id == request.ProductVariantId)
            ?? throw new InvalidOperationException("Product variant not found.");

        var cart = await GetOrCreateAsync(userId);
        var existingItem = cart.Items.FirstOrDefault(item => item.ProductVariantId == request.ProductVariantId);

        if (existingItem is null)
        {
            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductVariantId = variant.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = request.Quantity
            });
        }
        else
        {
            existingItem.Quantity += request.Quantity;
        }

        return await carts.SaveAsync(cart);
    }
}
