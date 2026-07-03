using Microsoft.AspNetCore.Mvc;

namespace EloisStore.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public object Get() => new
    {
        service = "EloÍs Store API",
        status = "Healthy",
        timestamp = DateTimeOffset.UtcNow
    };
}
