using EloisStore.Api.Middlewares;
using Scalar.AspNetCore;

namespace EloisStore.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();

        app.MapOpenApi();
        app.MapScalarApiReference("/scalar");

        app.UseHttpsRedirection();
        app.MapControllers();
        app.MapHealthChecks("/health");

        return app;
    }
}
