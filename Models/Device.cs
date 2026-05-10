namespace SimpleApp.Core.Models;

public class Device
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public DeviceStatus Status { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public DateTime LastUpdate { get; set; }
    public List<TagConfig> Tags { get; set; } = new();
}
