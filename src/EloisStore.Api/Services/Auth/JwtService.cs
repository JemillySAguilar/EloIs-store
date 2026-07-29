using EloisStore.Api.Models.Cart;

namespace EloisStore.Api.Services.Auth;

public sealed class JwtService
{
    public string CreateAccessToken(User user) =>
        Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + "." + user.Id;
}
