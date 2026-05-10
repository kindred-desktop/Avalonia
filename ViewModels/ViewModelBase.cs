using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleApp.Desktop.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _title = string.Empty;
}
