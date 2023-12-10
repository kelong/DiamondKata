using DiamondKata.Abstraction;
using DiamondKata.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
var appSettingsFile = environment switch
{
    "Production" => "appsettings.prod.json",
    "Test" => "appsettings.test.json",
    _ => "appsettings.json"
};

var configuration = new ConfigurationBuilder().AddJsonFile(appSettingsFile);
configuration.Build();

static IHost AppStartup(string appSettingsFile)
{
    var builder = new ConfigurationBuilder();

    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(appSettingsFile, optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            services.AddScoped<ILettersProvider, LettersProvider>()
                .AddScoped<IDiamondService, DiamondService>()
                .AddLogging(x => {x.AddSerilog(Log.Logger);})
                .AddSingleton<ILogger>(_ => Log.Logger);
        });

    host.UseSerilog((hostContext, services, configuration) => {
        configuration.WriteTo.Console();
    });
    
    var result = host.Build();
    
    return result;
}

var host = AppStartup(appSettingsFile);

var diamondService = ActivatorUtilities.CreateInstance<DiamondService>(host.Services);


if (args.Any())
{
    diamondService.DrawDiamondBasedOnLetter(args[0]);
}
else
{
    Log.Logger.Error("No arguments provided, please provide letter.");
}
