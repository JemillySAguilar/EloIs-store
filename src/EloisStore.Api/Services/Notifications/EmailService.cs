namespace EloisStore.Api.Services.Notifications;

public sealed class EmailService(ILogger<EmailService> logger)
{
    public Task SendAsync(string to, string subject, string body)
    {
        logger.LogInformation("Email simulated to {To}: {Subject} - {Body}", to, subject, body);
        return Task.CompletedTask;
    }
}
