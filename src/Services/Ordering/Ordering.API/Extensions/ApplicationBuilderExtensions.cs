namespace Ordering.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomMvc(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllers();
        });

        return app;
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Ordering.API V1");
            });

        return app;
    }
}