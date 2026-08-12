using System.Security.Claims;

namespace EloisStore.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var userId)
            ? userId
            : throw new UnauthorizedAccessException("Authenticated user identifier is invalid.");
    }

    public static bool IsAdmin(this ClaimsPrincipal principal) => principal.IsInRole("Admin");

    public static bool CanAccessUser(this ClaimsPrincipal principal, Guid userId) =>
        principal.IsAdmin() || principal.GetUserId() == userId;
}