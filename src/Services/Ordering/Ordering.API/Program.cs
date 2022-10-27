var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCustomMvc();
    builder.Services.AddCustomSwagger();
    builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
    builder.Host.UseSerilog();
    
    var app = builder.Build();
    app.UseCustomSwagger();
    app.UseCustomMvc();

    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    app.Run();

    return 0;
}
catch(Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);

    return 1;
}
finally
{
    Log.CloseAndFlush();
}

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.WithProperty("ApplicationContext", Program.AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables();

    return builder.Build();
}

public partial class Program
{
    public static string AssemblyName = typeof(Program).Assembly.FullName;
    public static string AppName = AssemblyName.Substring(0,AssemblyName.IndexOf(','));
}