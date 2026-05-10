# SimpleApp

一个基于 Avalonia 的跨平台桌面文本处理工具。

## 技术栈

| 技术 | 版本 |
|------|------|
| .NET | 8.0 |
| Avalonia | 11.0.0 |
| Serilog (日志) | 4.0.0 |

## 功能特性

- 文本字符数统计
- 文本单词数统计
- 文本反转
- 处理历史记录
- 跨平台支持 (Windows/macOS/Linux)
- 完整的日志系统
- 全局异常捕获

## 项目结构

```
SimpleApp/
├── App.axaml          # 应用样式和主题定义
├── App.axaml.cs       # 主应用入口
├── MainWindow.axaml   # 主窗口 XAML 定义
├── MainWindow.axaml.cs # 主窗口代码隐藏
├── Program.cs          # 程序入口、日志配置、全局异常处理
├── appsettings.json    # 应用程序配置
├── SimpleApp.csproj    # 项目文件
└── publish.sh          # 跨平台发布脚本
```

## 构建

### 环境要求

- .NET 8.0 SDK 或更高版本

### 开发模式运行

```bash
dotnet run
```

### Release 构建

```bash
dotnet build -c Release
```

## 发布

### 一键发布所有平台

```bash
chmod +x publish.sh
./publish.sh
```

这将生成三个平台的发布文件在 `bin/Release/publish/` 目录下：

- `SimpleApp.app/` - macOS 应用包
- `win-x64/SimpleApp.exe` - Windows 单文件可执行
- `linux-x64/SimpleApp` - Linux 可执行文件

### macOS 应用包

macOS 版本以 `.app` 应用包形式发布，分发时需要包含整个 `SimpleApp.app` 目录。

### 单平台发布

```bash
# macOS
dotnet publish -c Release -r osx-x64 --self-contained true -o ./dist/macos

# Windows
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./dist/windows

# Linux
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./dist/linux
```

## 运行

### macOS

```bash
# 方式一：双击 SimpleApp.app

# 方式二：命令行运行
open ./dist/SimpleApp.app
# 或
./dist/SimpleApp.app/Contents/MacOS/SimpleApp
```

### Windows

```bash
./dist/windows/SimpleApp.exe
```

### Linux

```bash
./dist/linux/SimpleApp
```

## 日志

日志文件保存在系统用户目录：

- **macOS**: `~/Library/Application Logs/SimpleApp/Logs/`
- **Windows**: `%LOCALAPPDATA%\SimpleApp\Logs\`
- **Linux**: `~/.local/share/SimpleApp/Logs/`

日志每日滚动，保留最近 7 天。

## 配置

`appsettings.json` 文件包含应用配置：

```json
{
  "AppSettings": {
    "Version": "1.0.0",
    "MaxHistoryItems": 100,
    "AutoSaveHistory": true,
    "WindowWidth": 500,
    "WindowHeight": 400
  },
  "Logging": {
    "MinimumLevel": "Debug",
    "RetainDays": 7
  }
}
```

## 许可证

MIT
