using EloisStore.Api.Services.Auth;
using Xunit;

namespace EloisStore.UnitTests;

public sealed class PasswordServiceTests
{
    [Fact]
    public void Verify_ReturnsTrue_WhenPasswordMatchesHash()
    {
        var service = new PasswordService();
        var hash = service.Hash("secret");

        Assert.True(service.Verify("secret", hash));
    }
}
