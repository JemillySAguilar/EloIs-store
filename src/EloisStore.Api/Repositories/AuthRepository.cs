using EloisStore.Api.Data;
using EloisStore.Api.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class AuthRepository(EloisStoreDbContext dbContext)
{
    public Task<Cart?> FindByUserIdAsync(Guid userId) =>
        dbContext.Carts.Include(cart => cart.Items).FirstOrDefaultAsync(cart => cart.UserId == userId);

  public Task<User?> FindByEmailAsync(string email) =>
        dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

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

    public async Task<User> AddAsync(User user)
    {
         dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}
