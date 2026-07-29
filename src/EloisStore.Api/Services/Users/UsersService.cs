using EloisStore.Api.Models.Cart;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Users;

public sealed class UsersService(UserRepository userRepository)
{
    public Task <List <User>> GetUsersAsync()
    {
        var users = userRepository.ListUsersAsync();
        return users;
    }
}
