using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections.Generic;
using System.Text;

namespace SimpleApp
{
    public partial class MainWindow : Window
    {
        private TextBox _inputTextBox = null!;
        private TextBox _historyTextBox = null!;
        private TextBlock _resultLabel = null!;
        private List<string> _history = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);

            _inputTextBox = this.FindControl<TextBox>("InputTextBox")!;
            _historyTextBox = this.FindControl<TextBox>("HistoryTextBox")!;
            _resultLabel = this.FindControl<TextBlock>("ResultLabel")!;

            var submitButton = this.FindControl<Button>("SubmitButton")!;
            var clearButton = this.FindControl<Button>("ClearButton")!;

            submitButton.Click += SubmitButton_Click;
            clearButton.Click += ClearButton_Click;
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
