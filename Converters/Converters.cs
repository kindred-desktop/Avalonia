using Avalonia.Data.Converters;
using Avalonia.Media;
using SimpleApp.Core.Models;

namespace SimpleApp.Desktop.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DeviceStatus status)
        {
            return status switch
            {
                DeviceStatus.Online => new SolidColorBrush(Color.Parse("#22C55E")),
                DeviceStatus.Running => new SolidColorBrush(Color.Parse("#22C55E")),
                DeviceStatus.Idle => new SolidColorBrush(Color.Parse("#F59E0B")),
                DeviceStatus.Warning => new SolidColorBrush(Color.Parse("#F59E0B")),
                DeviceStatus.Fault => new SolidColorBrush(Color.Parse("#EF4444")),
                DeviceStatus.Maintenance => new SolidColorBrush(Color.Parse("#8B5CF6")),
                DeviceStatus.Offline => new SolidColorBrush(Color.Parse("#94A3B8")),
                _ => new SolidColorBrush(Color.Parse("#94A3B8"))
            };
        }
        return new SolidColorBrush(Color.Parse("#94A3B8"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class AlarmLevelToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is AlarmLevel level)
        {
            return level switch
            {
                AlarmLevel.Critical => new SolidColorBrush(Color.Parse("#EF4444")),
                AlarmLevel.Warning => new SolidColorBrush(Color.Parse("#F59E0B")),
                AlarmLevel.Info => new SolidColorBrush(Color.Parse("#3B82F6")),
                _ => new SolidColorBrush(Color.Parse("#94A3B8"))
            };
        }
        return new SolidColorBrush(Color.Parse("#94A3B8"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class DeviceTypeToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DeviceType type)
        {
            return type switch
            {
                DeviceType.Motor => "⚙️",
                DeviceType.Pump => "🔴",
                DeviceType.Valve => "🔵",
                DeviceType.Sensor => "📊",
                DeviceType.Tank => "🛢️",
                DeviceType.Conveyor => "➡️",
                DeviceType.Compressor => "💨",
                DeviceType.Generator => "⚡",
                _ => "📦"
            };
        }
        return "📦";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BoolToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "是" : "否";
        }
        return "否";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
