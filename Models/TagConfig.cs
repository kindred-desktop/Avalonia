namespace SimpleApp.Core.Models;

public class TagConfig
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public double? UpperLimit { get; set; }
    public double? LowerLimit { get; set; }
    public double? Deadband { get; set; }
}
