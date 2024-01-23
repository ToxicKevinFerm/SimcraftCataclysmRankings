using ClassicCataSimcraft.Options;
using Microsoft.Extensions.Options;

namespace ClassicCataSimcraft;

public class SimulationManager(SimulationRunner runner, IOptions<SimcraftOptions> options)
{
    public async Task RunSimulationsAsync()
    {
        var sims = Directory.EnumerateFiles(options.Value.InputPath, "*.simc");
        var tasks = new List<Task>();

        foreach (var sim in sims)
        {
            var fileName = Path.GetFileNameWithoutExtension(sim);
            tasks.Add(RunSimulationAsync(fileName));
        }

        await Task.WhenAll(tasks);
        
        var result = runner.ReadResult();
    }

    private async Task RunSimulationAsync(string fileName)
    {
        await Task.Run(() => runner.RunSimulation(fileName));
    }
}