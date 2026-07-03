using EloisStore.Api.Configurations;
using EloisStore.Api.Data;
using EloisStore.Api.Repositories;
using EloisStore.Api.Services.Auth;
using EloisStore.Api.Services.Cart;
using EloisStore.Api.Services.Catalog;
using EloisStore.Api.Services.Notifications;
using EloisStore.Api.Services.Orders;
using EloisStore.Api.Services.Payments;
using Microsoft.EntityFrameworkCore;

namespace EloisStore.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();

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

        services.AddScoped<ProductRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<CartRepository>();
        services.AddScoped<OrderRepository>();
        services.AddScoped<PaymentRepository>();
        services.AddScoped<UserRepository>();

        services.AddHealthChecks();

        return services;
    }
}
