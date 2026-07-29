using EloisStore.Api.Data;
using EloisStore.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiPipeline();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EloisStoreDbContext>();
    await DatabaseSeeder.SeedAsync(dbContext);
}

app.Run();

public partial class Program;
