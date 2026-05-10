using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleApp.Desktop.Services;

namespace SimpleApp.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    [ObservableProperty]
    private string _selectedMenuItem = "Dashboard";

    [ObservableProperty]
    private string _currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    [ObservableProperty]
    private int _activeAlarmCount;

    [ObservableProperty]
    private int _totalDeviceCount;

    [ObservableProperty]
    private int _onlineDeviceCount;

    private readonly DashboardViewModel _dashboardViewModel;
    private readonly DeviceListViewModel _deviceListViewModel;
    private readonly TrendsViewModel _trendsViewModel;
    private readonly AlarmListViewModel _alarmListViewModel;
    private readonly SettingsViewModel _settingsViewModel;

    public MainWindowViewModel()
    {
        Title = "IAStudio - 工业自动控制系统";
        _dashboardViewModel = new DashboardViewModel();
        _deviceListViewModel = new DeviceListViewModel();
        _trendsViewModel = new TrendsViewModel();
        _alarmListViewModel = new AlarmListViewModel();
        _settingsViewModel = new SettingsViewModel();
        CurrentPage = _dashboardViewModel;

        StartTimeUpdate();
        _ = UpdateStatisticsAsync();
    }

    private void StartTimeUpdate()
    {
        var timer = new System.Timers.Timer(1000);
        timer.Elapsed += (_, _) => CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        timer.Start();
    }

    private async Task UpdateStatisticsAsync()
    {
        while (true)
        {
            try
            {
                var devices = await ServiceLocator.DeviceService.GetAllDevicesAsync();
                var deviceList = devices.ToList();
                TotalDeviceCount = deviceList.Count;
                OnlineDeviceCount = deviceList.Count(d => d.Status != Core.Models.DeviceStatus.Offline);

                var alarms = await ServiceLocator.AlarmService.GetAlarmsAsync(DateTime.Today, DateTime.Now);
                ActiveAlarmCount = alarms.Count(a => !a.IsAcknowledged);
            }
            catch { }

            await Task.Delay(5000);
        }
    }

    [RelayCommand]
    private void NavigateTo(string pageName)
    {
        SelectedMenuItem = pageName;
        CurrentPage = pageName switch
        {
            "Dashboard" => _dashboardViewModel,
            "Devices" => _deviceListViewModel,
            "Trends" => _trendsViewModel,
            "Alarms" => _alarmListViewModel,
            "Settings" => _settingsViewModel,
            _ => _dashboardViewModel
        };
    }
}
