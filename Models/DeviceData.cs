namespace SimpleApp.Core.Models;

public class DeviceData
{
    public string DeviceId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public List<TagValue> Values { get; set; } = new();
}
