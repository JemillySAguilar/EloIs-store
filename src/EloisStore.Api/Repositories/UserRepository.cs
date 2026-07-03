using EloisStore.Api.Data;
using EloisStore.Api.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class UserRepository(EloisStoreDbContext dbContext)
{
    public Task<User?> FindByEmailAsync(string email) =>
        dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<User> AddAsync(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}
