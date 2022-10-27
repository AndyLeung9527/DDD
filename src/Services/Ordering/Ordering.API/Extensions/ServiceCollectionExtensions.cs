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
}

