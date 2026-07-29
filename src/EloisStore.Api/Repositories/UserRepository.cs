using EloisStore.Api.Data;
using EloisStore.Api.Models.Cart;
using EloisStore.Api.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Repositories;

public sealed class UserRepository(EloisStoreDbContext dbContext)
{
    public async Task<List<User>> ListUsersAsync () => await dbContext.Users.ToListAsync();
   
}

