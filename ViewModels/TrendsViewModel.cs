using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleApp.Core.Models;
using SimpleApp.Desktop.Services;

namespace SimpleApp.Desktop.ViewModels;

public partial class TrendsViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Device> _availableDevices = new();

    [ObservableProperty]
    private Device? _selectedDevice;

    [ObservableProperty]
    private ObservableCollection<TagConfig> _availableTags = new();

    [ObservableProperty]
    private TagConfig? _selectedTag;

    [ObservableProperty]
    private ObservableCollection<string> _timeRanges = new()
    {
        "实时", "1小时", "6小时", "24小时", "7天"
    };

    [ObservableProperty]
    private string _selectedTimeRange = "1小时";

    [ObservableProperty]
    private ObservableCollection<DataPoint> _chartData = new();

    public TrendsViewModel()
    {
        Title = "趋势图";
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var devices = await ServiceLocator.DeviceService.GetAllDevicesAsync();
        AvailableDevices = new ObservableCollection<Device>(devices);

        if (AvailableDevices.Any())
        {
            SelectedDevice = AvailableDevices.First();
        }
    }

    partial void OnSelectedDeviceChanged(Device? value)
    {
        if (value != null)
        {
            AvailableTags = new ObservableCollection<TagConfig>(value.Tags);
            if (AvailableTags.Any())
            {
                SelectedTag = AvailableTags.First();
            }
        }
    }

    partial void OnSelectedTimeRangeChanged(string value) => _ = LoadDataAsync();

    partial void OnSelectedTagChanged(TagConfig? value) => _ = LoadDataAsync();

    private async Task LoadDataAsync()
    {
        if (SelectedDevice == null || SelectedTag == null) return;

        var (from, to) = GetTimeRange();
        var data = await ServiceLocator.DataService.GetHistoricalDataAsync(
            SelectedDevice.Id, from, to);

        var filtered = data.Where(d => d.TagId == SelectedTag.Id);
        ChartData = new ObservableCollection<DataPoint>(filtered.OrderBy(d => d.Timestamp).Take(100));
    }

    private (DateTime from, DateTime to) GetTimeRange()
    {
        var to = DateTime.Now;
        var from = SelectedTimeRange switch
        {
            "实时" => to.AddMinutes(-5),
            "1小时" => to.AddHours(-1),
            "6小时" => to.AddHours(-6),
            "24小时" => to.AddHours(-24),
            "7天" => to.AddDays(-7),
            _ => to.AddHours(-1)
        };
        return (from, to);
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDataAsync();
    }
}
