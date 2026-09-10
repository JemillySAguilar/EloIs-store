namespace EloisStore.Api.Models.Cart;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string Password,
    string Phone,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string PostalCode);
