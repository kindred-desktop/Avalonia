using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleApp.Desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _modbusIp = "192.168.1.100";

    [ObservableProperty]
    private int _modbusPort = 502;

    [ObservableProperty]
    private int _pollingInterval = 1000;

    [ObservableProperty]
    private int _historyRetentionDays = 30;

    [ObservableProperty]
    private bool _enableAlarmSound = true;

    [ObservableProperty]
    private string _alarmSoundFile = "default.wav";

    public SettingsViewModel()
    {
        Title = "设置";
    }

    [RelayCommand]
    private void SaveSettings()
    {
    }
}
