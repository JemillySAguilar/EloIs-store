namespace EloisStore.Api.Configurations;

public sealed class JwtSettings
{
    public string Issuer { get; set; } = "EloisStore";
    public string Audience { get; set; } = "EloisStore.Client";
    public string Secret { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 60;
}
