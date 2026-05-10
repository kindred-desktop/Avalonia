using Avalonia.Controls;
using SimpleApp.Desktop.ViewModels;

namespace SimpleApp.Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
