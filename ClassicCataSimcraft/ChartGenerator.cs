using System.Drawing;
using System.Globalization;
using ClassicCataSimcraft.Models;
using ScottPlot;
using static System.Int32;
using Color = ScottPlot.Color;

namespace ClassicCataSimcraft;

public static class ChartGenerator
{
    public static void CreateBarChart(List<SimulationResult> results, string filePath)
    {
        var plt = new ScottPlot.Plot();

        // Data preparation
        results = results.OrderBy(x => x.DPS).ToList();
        
        for (var i = 0; i < results.Count; i++)
        {
            var result = results[i];
            var error = result.DPSRange.Split("/")[0];
            TryParse(error, out var parsedError);
            var bar = new Bar()
            {
                Position = i, Value = result.DPS,
                FillColor = HexToScottPlotColor(result.PlayerInfo?.ColorHex ?? "#00000"),
                Error = parsedError
            };
            var chart = plt.Add.Bar(bar);
            plt.Add.Text(result.DPS.ToString(CultureInfo.InvariantCulture), result.DPS+100, i+0.3);
            chart.Horizontal = true;
            
        }
        var ticks = results.Select((result, index) => new Tick(index, result.PlayerInfo?.FileName ?? "")).ToArray();

        plt.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
        plt.Axes.Left.MajorTickStyle.Length = 0;
        // Set axis limits if necessary
        plt.Axes.Left.TickLabelStyle.Bold = true;
        plt.HideGrid();
        
        
        plt.Title("Cataclysm 4.3.3 (15354) T11 Heroic BIS");
        plt.Axes.Margins(left: 0);
        plt.SavePng($"{filePath}/result.png", 1920,1080);
    }
    private static Color HexToScottPlotColor(string hex)
    {
        var argb = Parse(hex.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
        var clr = System.Drawing.Color.FromArgb(argb);
        return new Color(clr.R, clr.G, clr.B);
    }
    private static double ParseRange(string dpsRange)
    {
        return 0;
    }
}