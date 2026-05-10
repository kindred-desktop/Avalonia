namespace SimpleApp.Core.Models;

public class Alarm
{
    public string Id { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string TagId { get; set; } = string.Empty;
    public AlarmLevel Level { get; set; }
    public AlarmType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public double TriggerValue { get; set; }
    public double Threshold { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
    public bool IsAcknowledged { get; set; }
}
