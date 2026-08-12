using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EloisStore.Api.Configurations;
using EloisStore.Api.Models.Cart;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EloisStore.Api.Services.Auth;

public sealed class JwtService(IOptions<JwtSettings> options)
{
    private readonly JwtSettings settings = options.Value;

    public string CreateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.AccessTokenMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}