namespace EloisStore.Api.Models.Auth;

public sealed record AuthResponse(Guid UserId, string Name, string Email, string Role, string AccessToken);
