using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleApp.Core.Models;
using SimpleApp.Desktop.Services;

namespace SimpleApp.Desktop.ViewModels;

public partial class AlarmListViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Alarm> _alarms = new();

    [ObservableProperty]
    private Alarm? _selectedAlarm;

    [ObservableProperty]
    private string _selectedLevelFilter = "全部";

    [ObservableProperty]
    private string _selectedStatusFilter = "全部";

    [ObservableProperty]
    private ObservableCollection<Device> _devices = new();

    [ObservableProperty]
    private Device? _selectedDevice;

    public ObservableCollection<string> LevelFilters { get; } = new()
    {
        "全部", "Critical", "Warning", "Info"
    };

    public ObservableCollection<string> StatusFilters { get; } = new()
    {
        "全部", "未确认", "已确认"
    };

    public AlarmListViewModel()
    {
        Title = "报警记录";
        _ = LoadDataAsync();
    }

    partial void OnSelectedLevelFilterChanged(string value) => _ = ApplyFiltersAsync();
    partial void OnSelectedStatusFilterChanged(string value) => _ = ApplyFiltersAsync();
    partial void OnSelectedDeviceChanged(Device? value) => _ = ApplyFiltersAsync();

    private async Task LoadDataAsync()
    {
        while (true)
        {
            try
            {
                var devices = await ServiceLocator.DeviceService.GetAllDevicesAsync();
                Devices = new ObservableCollection<Device>(devices);

                var alarms = await ServiceLocator.AlarmService.GetAlarmsAsync(
                    DateTime.Today.AddDays(-7), DateTime.Now);
                Alarms = new ObservableCollection<Alarm>(alarms.OrderByDescending(a => a.Timestamp));

                await ApplyFiltersAsync();
            }
            catch { }

            await Task.Delay(3000);
        }
    }

    private async Task ApplyFiltersAsync()
    {
        var from = DateTime.Today.AddDays(-7);
        var to = DateTime.Now;
        var allAlarms = await ServiceLocator.AlarmService.GetAlarmsAsync(from, to);
        var filtered = allAlarms.AsEnumerable();

        if (SelectedLevelFilter != "全部")
        {
            var level = Enum.Parse<AlarmLevel>(SelectedLevelFilter);
            filtered = filtered.Where(a => a.Level == level);
        }

        if (SelectedStatusFilter == "未确认")
            filtered = filtered.Where(a => !a.IsAcknowledged);
        else if (SelectedStatusFilter == "已确认")
            filtered = filtered.Where(a => a.IsAcknowledged);

        if (SelectedDevice != null)
            filtered = filtered.Where(a => a.DeviceId == SelectedDevice.Id);

        Alarms = new ObservableCollection<Alarm>(filtered.OrderByDescending(a => a.Timestamp));
    }

    [RelayCommand]
    private async Task AcknowledgeAlarmAsync(Alarm? alarm)
    {
        if (alarm == null) return;

        await ServiceLocator.AlarmService.AcknowledgeAlarmAsync(alarm.Id);
        alarm.IsAcknowledged = true;
        alarm.AcknowledgedAt = DateTime.Now;
        alarm.AcknowledgedBy = "Operator";
    }

    [RelayCommand]
    private async Task AcknowledgeAllAsync()
    {
        var unacknowledged = Alarms.Where(a => !a.IsAcknowledged);
        foreach (var alarm in unacknowledged)
        {
            await ServiceLocator.AlarmService.AcknowledgeAlarmAsync(alarm.Id);
        }
        await ApplyFiltersAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDataAsync();
    }
}
