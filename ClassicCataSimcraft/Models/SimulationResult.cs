namespace ClassicCataSimcraft.Models;

public class SimulationResult
{
    public PlayerInfo? PlayerInfo { get; set; }
    public double DPS { get; set; }
    public double DPSConvergence { get; set; }
    public string DPSRange { get; set; }
    public double ApM { get; set; }
}