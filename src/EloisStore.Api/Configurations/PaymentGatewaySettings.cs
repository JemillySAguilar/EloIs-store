namespace EloisStore.Api.Configurations;

public sealed class PaymentGatewaySettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public bool ForceApproval { get; set; } = true;
}
