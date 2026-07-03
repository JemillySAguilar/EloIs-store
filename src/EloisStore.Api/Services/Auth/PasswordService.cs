using System.Security.Cryptography;
using System.Text;

namespace EloisStore.Api.Services.Auth;

public sealed class PasswordService
{
    public string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string password, string passwordHash) =>
        Hash(password).Equals(passwordHash, StringComparison.OrdinalIgnoreCase);
}
