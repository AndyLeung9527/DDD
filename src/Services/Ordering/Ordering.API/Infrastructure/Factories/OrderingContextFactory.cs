namespace Ordering.API.Infrastructure.Factories;

public class OrderingContextFactory : IDesignTimeDbContextFactory<OrderingContext>
{
    public OrderingContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();

        optionsBuilder.UseMySql(config["ConnectionString"], MySqlServerVersion.LatestSupportedServerVersion, mysqlOptionsAction => mysqlOptionsAction.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));

        return new OrderingContext(optionsBuilder.Options); 
    }
}
