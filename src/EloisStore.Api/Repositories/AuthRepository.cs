using EloisStore.Api.Data;
using EloisStore.Api.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class AuthRepository(EloisStoreDbContext dbContext)
{
    public Task<Cart?> FindByUserIdAsync(Guid userId) =>
        dbContext.Carts.Include(cart => cart.Items).FirstOrDefaultAsync(cart => cart.UserId == userId);

    public async Task<Cart> SaveAsync(Cart cart)
    {
        if (dbContext.Entry(cart).State == EntityState.Detached)
        {
            dbContext.Carts.Add(cart);
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        return cart;
    }
}
