using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleApp.Core.Models;
using SimpleApp.Desktop.Services;

namespace SimpleApp.Desktop.ViewModels;

public partial class DeviceListViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Device> _devices = new();

    [ObservableProperty]
    private ObservableCollection<Device> _filteredDevices = new();

    [ObservableProperty]
    private Device? _selectedDevice;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedTypeFilter = "全部";

    [ObservableProperty]
    private string _selectedStatusFilter = "全部";

    [ObservableProperty]
    private DeviceData? _selectedDeviceData;

    public ObservableCollection<string> TypeFilters { get; } = new()
    {
        "全部", "Motor", "Pump", "Valve", "Sensor", "Tank", "Conveyor"
    };

    public ObservableCollection<string> StatusFilters { get; } = new()
    {
        "全部", "Online", "Running", "Idle", "Warning", "Fault"
    };

    public DeviceListViewModel()
    {
        Title = "设备列表";
        _ = LoadDevicesAsync();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilters();
    partial void OnSelectedTypeFilterChanged(string value) => ApplyFilters();
    partial void OnSelectedStatusFilterChanged(string value) => ApplyFilters();

    private async Task LoadDevicesAsync()
    {
        while (true)
        {
            try
            {
                var devices = await ServiceLocator.DeviceService.GetAllDevicesAsync();
                Devices = new ObservableCollection<Device>(devices);
                ApplyFilters();
            }
            catch { }

            await Task.Delay(3000);
        }
    }

    private void ApplyFilters()
    {
        var filtered = Devices.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(d =>
                d.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                d.Code.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                d.Location.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        if (SelectedTypeFilter != "全部")
            filtered = filtered.Where(d => d.Type.ToString() == SelectedTypeFilter);

        if (SelectedStatusFilter != "全部")
            filtered = filtered.Where(d => d.Status.ToString() == SelectedStatusFilter);

        FilteredDevices = new ObservableCollection<Device>(filtered);
    }

    [RelayCommand]
    private async Task SelectDeviceAsync(Device? device)
    {
        if (device == null) return;

        SelectedDevice = device;
        SelectedDeviceData = await ServiceLocator.DeviceService.GetDeviceDataAsync(device.Id);
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        var devices = await ServiceLocator.DeviceService.GetAllDevicesAsync();
        Devices = new ObservableCollection<Device>(devices);
        ApplyFilters();
    }
}
