namespace EloisStore.Api.Models.Cart;

public sealed record AuthResponse(Guid UserId, string Name, string Email, string Role, string AccessToken);
