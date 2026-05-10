using SimpleApp.Core.Drivers;
using SimpleApp.Core.Services.Interfaces;

namespace SimpleApp.Desktop.Services;

public static class ServiceLocator
{
    private static IDeviceService? _deviceService;
    private static IDataService? _dataService;
    private static IAlarmService? _alarmService;

    public static IDeviceService DeviceService => _deviceService ??= new MockDeviceService();
    public static IDataService DataService => _dataService ??= new MockDataService(DeviceService);
    public static IAlarmService AlarmService => _alarmService ??= new MockAlarmService(DeviceService);
}
