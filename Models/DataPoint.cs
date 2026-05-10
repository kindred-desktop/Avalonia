namespace SimpleApp.Core.Models;

public class DataPoint
{
    public string DeviceId { get; set; } = string.Empty;
    public string TagId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
}
