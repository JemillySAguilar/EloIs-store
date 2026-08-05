using EloisStore.Api.Configurations;
using EloisStore.Api.Data;
using EloisStore.Api.Repositories;
using EloisStore.Api.Services.Auth;
using EloisStore.Api.Services.Cart;
using EloisStore.Api.Services.Catalog;
using EloisStore.Api.Services.Notifications;
using EloisStore.Api.Services.Orders;
using EloisStore.Api.Services.Payments;
using EloisStore.Api.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public const string FrontendCorsPolicy = "FrontendCorsPolicy";

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
                policy
                    .SetIsOriginAllowed(IsLocalFrontendOrigin)
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.Configure<PaymentGatewaySettings>(configuration.GetSection("PaymentGateway"));

        services.AddDbContext<EloisStoreDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<AuthService>();
        services.AddScoped<JwtService>();
        services.AddScoped<PasswordService>();
        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<ProductSearchService>();
        services.AddScoped<CartService>();
        services.AddScoped<OrderService>();
        services.AddScoped<CheckoutService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<PaymentGatewayClient>();
        services.AddScoped<EmailService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<UsersService>();

        services.AddScoped<AuthRepository>();
        services.AddScoped<ProductRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<CartRepository>();
        services.AddScoped<OrderRepository>();
        services.AddScoped<PaymentRepository>();
        services.AddScoped<UserRepository>();

        services.AddHealthChecks();

        return services;
    }

    private static bool IsLocalFrontendOrigin(string origin)
    {
        if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
        {
            return false;
        }

        return uri.Scheme is "http" or "https"
            && (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                || uri.Host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase));
    }
}
