using ClassicCataSimcraft;
using ClassicCataSimcraft.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup DI
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetService<IOptions<SimcraftOptions>>();
        var simulationRunner = serviceProvider.GetService<SimulationRunner>();
        var simulationManager = new SimulationManager(simulationRunner, options);

        await simulationManager.RunSimulationsAsync();

        Console.WriteLine("All simulations completed.");
    }

    private static void ConfigureServices(IServiceCollection services)
    {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<SimulationRunner>();
        services.AddSingleton<ReportsReader>();
        // Register configuration
        services.Configure<SimcraftOptions>(configuration.GetSection("SimcraftOptions"));

    }
}