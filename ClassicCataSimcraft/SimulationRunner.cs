using System.Diagnostics;
using ClassicCataSimcraft.Options;
using Microsoft.Extensions.Options;

namespace ClassicCataSimcraft;

public class SimulationRunner(IOptions<SimcraftOptions> options, ReportsReader reportsReader)
{
    public void RunSimulation(string fileName)
    {
        var inputFilePath = Path.Combine(options.Value.InputPath, $"{fileName}.simc");
        var outputFilePath = Path.Combine(options.Value.OutputPath, $"{fileName}.result.txt");

        var command = Path.Combine(options.Value.SimcPath, $"simc.exe");
        var arguments = $"\"{inputFilePath}\" iterations=1000";

        using var process = new Process();
        process.StartInfo.FileName = command;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        // Read output to a string
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Write output to a file
        File.WriteAllText(outputFilePath, output);
    }
    
    public string ReadResult()
    {
        var simulationResults = reportsReader.ReadAndProcessReports();

        foreach (var result in simulationResults)
        {
            Console.WriteLine($"DPS: {result.DPS}, DPS Convergence: {result.DPSConvergence}, DPS Range: {result.DPSRange}, ApM: {result.ApM}");
        }

        return "true";
    }
    
}