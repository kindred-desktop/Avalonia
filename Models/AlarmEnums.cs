namespace SimpleApp.Core.Models;

public enum AlarmLevel
{
    Info,
    Warning,
    Critical
}

public enum AlarmType
{
    HighHigh,
    High,
    Low,
    LowLow,
    Deviation,
    RateOfChange
}
