using EloisStore.Api.Models.Cart;
using EloisStore.Api.Repositories;

namespace EloisStore.Api.Services.Auth;

public sealed class AuthService(AuthRepository users, PasswordService passwords, JwtService jwt)
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await users.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("E-mail already registered.");
        }

        var user = await users.AddAsync(new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwords.Hash(request.Password)
        });

        return ToResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await users.FindByEmailAsync(request.Email)
            ?? throw new InvalidOperationException("Invalid credentials.");

        if (!passwords.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        return ToResponse(user);
    }

    private AuthResponse ToResponse(User user) =>
        new(user.Id, user.Name, user.Email, user.Role, jwt.CreateAccessToken(user));
}
