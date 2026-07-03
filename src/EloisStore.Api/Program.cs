using EloisStore.Api.Data;
using EloisStore.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiPipeline();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider.GetRequiredService<EloisStoreDbContext>());
}

app.Run();

public partial class Program;
