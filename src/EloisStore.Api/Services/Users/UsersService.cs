using EloisStore.Api.Models.Auth;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Users;

public sealed class UsersService(UserRepository userRepository)
{
    public async Task<List<UserResponse>> GetUsersAsync()
    {
        var users = await userRepository.ListUsersAsync();
        return users
            .Select(user => new UserResponse(user.Id, user.Name, user.Email, user.Role, user.CreatedAt))
            .ToList();
    }
}