using EloisStore.Api.Data;
using EloisStore.Api.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class AuthRepository(EloisStoreDbContext acessoBanco)
{
    public Task<Cart?> FindByUserIdAsync(Guid userId) =>
        acessoBanco.Carts.Include(cart => cart.Items).FirstOrDefaultAsync(cart => cart.UserId == userId);

  public Task<User?> FindByEmailAsync(string email) =>
        acessoBanco.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<Cart> SaveAsync(Cart cart)
    {
        if (acessoBanco.Entry(cart).State == EntityState.Detached)
        {
            acessoBanco.Carts.Add(cart);
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await acessoBanco.SaveChangesAsync();
        return cart;
    }

    public async Task<User> AddAsync(User user)
    {
         acessoBanco.Users.Add(user);
        await acessoBanco.SaveChangesAsync();
        return user;
    }
}
