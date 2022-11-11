namespace Ordering.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers()
            // Added for functional tests
            .AddApplicationPart(typeof(OrdersController).Assembly);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ordering HTTP API",
                Version = "v1",
                Description = "The Ordering Service HTTP API"
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomMediator(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Program).Assembly);
        services.AddScoped<IRequestHandler<IdentifiedCommand<CancelOrderCommand, bool>, bool>, IdentityCommandHandler<CancelOrderCommand, bool>>();
        services.AddScoped<IRequestHandler<IdentifiedCommand<ShipOrderCommand, bool>, bool>, IdentityCommandHandler<ShipOrderCommand, bool>>();

        return services;
    }

    public static IServiceCollection AddCustomInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRequestManager, RequestManager>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddDbContext<OrderingContext>(options =>
        {
            options.UseMySql(configuration["ConnectionString"], MySqlServerVersion.LatestSupportedServerVersion);
        });

        return services;
    }
}