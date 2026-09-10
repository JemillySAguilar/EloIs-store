namespace EloisStore.Api.Models.Auth;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    string Role,
    string Phone,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string PostalCode,
    DateTime CreatedAt);
