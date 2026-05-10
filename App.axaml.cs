using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using System.Collections.Generic;
using System.Text;

namespace SimpleApp
{
    public class App : Application
    {
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Styles.Add(new FluentTheme());
                desktop.MainWindow = new MainWindow();
            }
        }
    }

    public class MainWindow : Window
    {
        private TextBox _inputTextBox = null!;
        private TextBox _historyTextBox = null!;
        private TextBlock _resultLabel = null!;
        private List<string> _history = new List<string>();

        public MainWindow()
        {
            Title = "简单文本处理工具";
            Width = 500;
            Height = 400;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CanResize = false;

            var panel = new StackPanel();

            var inputLabel = new TextBlock
            {
                Text = "输入文本：",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                FontSize = 14
            };
            panel.Children.Add(inputLabel);

            _inputTextBox = new TextBox
            {
                Watermark = "请输入要处理的文本...",
                Height = 30
            };
            panel.Children.Add(_inputTextBox);

            var btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 10
            };

            var submitButton = new Button { Content = "提交", Width = 100, Height = 30 };
            submitButton.Click += SubmitButton_Click;
            btnPanel.Children.Add(submitButton);

            var clearButton = new Button { Content = "清空历史", Width = 100, Height = 30 };
            clearButton.Click += ClearButton_Click;
            btnPanel.Children.Add(clearButton);

            panel.Children.Add(btnPanel);

            _resultLabel = new TextBlock
            {
                Text = "结果：",
                Foreground = Brushes.Green,
                FontSize = 12
            };
            panel.Children.Add(_resultLabel);

            var historyLabel = new TextBlock
            {
                Text = "处理历史：",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                FontSize = 14
            };
            panel.Children.Add(historyLabel);

            _historyTextBox = new TextBox
            {
                IsReadOnly = true,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 200
            };
            panel.Children.Add(_historyTextBox);

            Content = panel;
            Padding = new Thickness(20);
        }

        private void SubmitButton_Click(object? sender, RoutedEventArgs e)
        {
            string input = _inputTextBox.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(input))
            {
                var dialog = new Window
                {
                    Title = "提示",
                    Width = 300,
                    Height = 120,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    CanResize = false,
                    Content = new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Children =
                        {
                            new TextBlock { Text = "请输入文本！", Margin = new Thickness(0, 0, 0, 15) },
                            new Button { Content = "确定", Width = 80, HorizontalAlignment = HorizontalAlignment.Center }
                        }
                    }
                };
                dialog.ShowDialog(this);
                return;
            }

            StringBuilder result = new StringBuilder();
            result.AppendLine($"原始文本：{input}");
            result.AppendLine($"字符数：{input.Length}");
            result.AppendLine($"单词数：{GetWordCount(input)}");
            result.AppendLine($"反转文本：{ReverseText(input)}");
            result.AppendLine(new string('-', 30));

            _resultLabel.Text = $"结果：字符数={input.Length}, 单词数={GetWordCount(input)}";
            _history.Add(result.ToString());
            UpdateHistoryDisplay();

            _inputTextBox.Text = "";
        }

        private void ClearButton_Click(object? sender, RoutedEventArgs e)
        {
            _history.Clear();
            _historyTextBox.Text = "";
            _resultLabel.Text = "结果：";
        }

        private void UpdateHistoryDisplay()
        {
            _historyTextBox.Text = string.Join("\n", _history);
        }

        private string ReverseText(string text)
        {
            char[] chars = text.ToCharArray();
            System.Array.Reverse(chars);
            return new string(chars);
        }

        private int GetWordCount(string text)
        {
            return text.Split(new[] { ' ', ',', '.', '！', '？', '\n', '\r' },
                System.StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
