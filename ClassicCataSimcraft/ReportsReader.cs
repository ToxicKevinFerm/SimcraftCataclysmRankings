using System.Globalization;
using System.Text.RegularExpressions;
using ClassicCataSimcraft.Models;
using ClassicCataSimcraft.Options;
using Microsoft.Extensions.Options;
using static System.Text.RegularExpressions.Regex;

namespace ClassicCataSimcraft;

public partial class ReportsReader(IOptions<SimcraftOptions> options)
{
    public List<SimulationResult> ReadAndProcessReports()
    {
        var results = new List<SimulationResult>();
        var reports = Directory.GetFiles(options.Value.OutputPath, "*.txt");

        foreach (var report in reports)
        {
            var content = File.ReadAllText(report);
            var result = ExtractSimulationResult(content);
            if (result != null) results.Add(result);
        }
        ChartGenerator.CreateBarChart(results, options.Value.OutputPath);
        return results;
    }
    
    private static PlayerInfo? ExtractPlayerInfo(string content)
    {
        var regex = PlayerInfoRegex();
        var match = regex.Match(content);

        if (match.Success)
        {
            var playerInfo = new PlayerInfo
            {
                FileName = match.Groups[1].Value,
                Race = match.Groups[2].Value,
                Class = match.Groups[3].Value,
                Spec = match.Groups[4].Value
            };
            playerInfo.SetClassColor();
            return playerInfo;
            
        }

        return null;
    }
    
    [System.Text.RegularExpressions.GeneratedRegex(@"DPS: (\d+\.\d+).*?DPS-Error.*?DPS-Range=([\d/.%]+).*?DPS-Convergence=(\d+\.\d+)%.*?ApM=(\d+\.\d+)")]
    private static partial Regex ReportRegex();

    private SimulationResult? ExtractSimulationResult(string content)
    {
        var match = ReportRegex().Match(content);

        if (match.Success)
        {
            return new SimulationResult
            {
                DPS = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture),
                DPSRange = match.Groups[2].Value,
                DPSConvergence = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture),
                ApM = double.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture),
                PlayerInfo = ExtractPlayerInfo(content)
            };
        }

        return null;
    }

    [GeneratedRegex(@"Player: (\S+) (\S+) (\S+) (\S+)")]
    private static partial Regex PlayerInfoRegex();
}