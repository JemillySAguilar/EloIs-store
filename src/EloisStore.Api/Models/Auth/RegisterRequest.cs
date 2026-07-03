namespace EloisStore.Api.Models.Auth;

public sealed record RegisterRequest(string Name, string Email, string Password);
