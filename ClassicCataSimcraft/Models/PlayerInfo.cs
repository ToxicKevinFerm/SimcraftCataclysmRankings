namespace ClassicCataSimcraft.Models;

public class PlayerInfo
{
    private static readonly Dictionary<string, string> ClassColors = new Dictionary<string, string>
    {
        { "hunter", "#A9D271" },
        { "warrior", "#C79C6E" },
        { "paladin", "#F58CBA" },
        { "rogue", "#FFF569" },
        { "priest", "#FFFFFF" },
        { "deathknight", "#C41F3B" },
        { "shaman", "#0070DE" },
        { "mage", "#40C7EB" },
        { "warlock", "#8787ED" },
        { "druid", "#FF7D0A" }
    };

    public string ColorHex { get; set; } = "#000";
    public string FileName { get; set; }
    public string Race { get; set; }
    public string Class { get; set; }
    public string Spec { get; set; }
    
    public void SetClassColor()
    {
        if (ClassColors.TryGetValue(Class, out var color))
        {
            ColorHex = color;
        }
    }
}