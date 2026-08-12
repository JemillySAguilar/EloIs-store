namespace EloisStore.Api.Models.Auth;

public sealed record UserResponse(Guid Id, string Name, string Email, string Role, DateTime CreatedAt);