using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimpleApp.Core.Models;
using SimpleApp.Desktop.Services;

namespace SimpleApp.Desktop.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _totalDevices;

    [ObservableProperty]
    private int _onlineDevices;

    [ObservableProperty]
    private int _runningDevices;

    [ObservableProperty]
    private int _warningDevices;

    [ObservableProperty]
    private int _activeAlarms;

    [ObservableProperty]
    private int _criticalAlarms;

    [ObservableProperty]
    private ObservableCollection<Device> _recentDevices = new();

    [ObservableProperty]
    private ObservableCollection<Alarm> _recentAlarms = new();

    [ObservableProperty]
    private ObservableCollection<StatusItem> _statusDistribution = new();

    public DashboardViewModel()
    {
        Title = "仪表盘";
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        while (true)
        {
            try
            {
                var devices = await ServiceLocator.DeviceService.GetAllDevicesAsync();
                var deviceList = devices.ToList();

                TotalDevices = deviceList.Count;
                OnlineDevices = deviceList.Count(d => d.Status != DeviceStatus.Offline);
                RunningDevices = deviceList.Count(d => d.Status == DeviceStatus.Running);
                WarningDevices = deviceList.Count(d => d.Status == DeviceStatus.Warning || d.Status == DeviceStatus.Fault);

                RecentDevices = new ObservableCollection<Device>(
                    deviceList.OrderByDescending(d => d.LastUpdate).Take(5));

                StatusDistribution = new ObservableCollection<StatusItem>(
                    deviceList.GroupBy(d => d.Status)
                        .Select(g => new StatusItem
                        {
                            Status = g.Key.ToString(),
                            Count = g.Count(),
                            Percentage = (double)g.Count() / deviceList.Count * 100
                        }));

                var alarms = await ServiceLocator.AlarmService.GetAlarmsAsync(DateTime.Today, DateTime.Now);
                var alarmList = alarms.ToList();
                ActiveAlarms = alarmList.Count(a => !a.IsAcknowledged);
                CriticalAlarms = alarmList.Count(a => a.Level == AlarmLevel.Critical && !a.IsAcknowledged);

                RecentAlarms = new ObservableCollection<Alarm>(
                    alarmList.Where(a => !a.IsAcknowledged).Take(5));
            }
            catch { }

            await Task.Delay(2000);
        }
    }
}

public class StatusItem
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
