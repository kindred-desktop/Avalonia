# IAStudio (SimpleApp) - 工业自动控制系统

基于 **Avalonia UI 12.0.2** 和 **.NET 10** 的跨平台工业自动化监控系统（SCADA 模拟），支持设备监控、实时数据采集、报警管理与趋势分析。

## 功能特性

- **仪表盘** — 系统总览，展示关键指标（总设备数、在线设备、运行中、告警/故障）、报警概览与设备状态分布
- **设备列表** — 设备管理，支持按类型/状态筛选与关键字搜索，点击查看设备详情与实时测点数据
- **趋势数据** — 设备测点的历史趋势曲线与实时数据展示，支持多时间范围（实时/1小时/6小时/24小时/7天）
- **报警列表** — 报警管理，支持按等级/状态筛选，单条或批量确认，实时报警推送
- **系统设置** — Modbus TCP 通信参数配置、轮询间隔、历史数据保留天数、报警声音开关

## 技术栈

| 组件 | 技术 | 版本 |
|------|------|------|
| UI 框架 | Avalonia UI | 12.0.2 |
| 运行时 | .NET | 10.0 |
| 架构模式 | MVVM (CommunityToolkit.Mvvm) | 8.4.0 |
| 响应式编程 | System.Reactive | 6.0.1 |
| 日志框架 | Serilog | 4.0.0 |
| 界面主题 | Fluent Theme | — |
| CI/CD | GitHub Actions (linux-x64 / win-x64 / osx-x64 / osx-arm64) | — |

## 项目结构

```
SimpleApp/
├── Models/                  # 数据模型层
│   ├── Alarm.cs             # 报警模型
│   ├── AlarmEnums.cs        # 报警等级/类型枚举
│   ├── DataPoint.cs         # 数据点
│   ├── Device.cs            # 设备模型
│   ├── DeviceData.cs        # 设备实时数据
│   ├── DeviceStatus.cs      # 设备状态枚举
│   ├── DeviceType.cs        # 设备类型枚举
│   ├── TagConfig.cs         # 测点配置
│   └── TagValue.cs          # 测点值
├── ViewModels/              # 视图模型层
│   ├── ViewModelBase.cs     # 基类（ObservableObject）
│   ├── MainWindowViewModel.cs
│   ├── DashboardViewModel.cs
│   ├── DeviceListViewModel.cs
│   ├── TrendsViewModel.cs
│   ├── AlarmListViewModel.cs
│   └── SettingsViewModel.cs
├── Views/                   # 视图层 (XAML)
│   ├── MainWindow.axaml     # 主窗口（导航、状态栏）
│   ├── DashboardView.axaml  # 仪表盘
│   ├── DeviceListView.axaml # 设备列表（卡片布局）
│   ├── TrendsView.axaml     # 趋势曲线
│   ├── AlarmListView.axaml  # 报警列表
│   └── SettingsView.axaml   # 设置表单
├── Services/                # 服务接口层
│   ├── ServiceLocator.cs    # 服务定位器（静态单例）
│   └── Interfaces/
│       ├── IDeviceService.cs  # 设备管理接口
│       ├── IDataService.cs    # 数据采集接口
│       └── IAlarmService.cs   # 报警管理接口
├── Drivers/                 # 驱动程序层（Mock 模拟）
│   ├── MockDeviceRegistry.cs # 设备注册表（10 种工业设备）
│   ├── MockDeviceService.cs  # 模拟设备服务 + 数据生成
│   ├── MockDataService.cs    # 模拟数据服务（每秒生成，1h 内存缓存）
│   └── MockAlarmService.cs   # 模拟报警服务（随机触发）
├── Converters/              # XAML 值转换器
│   └── Converters.cs        # StatusToColor / AlarmLevelToColor / DeviceTypeToIcon / BoolToStr
├── scripts/
│   └── FixPermissions.command   # macOS 权限修复脚本
├── .github/workflows/
│   ├── ci.yml               # 代码推送 CI（build + test + 多平台发布）
│   └── build-release.yml    # 打标签自动构建 Release，上传 Artifact
├── appsettings.json         # 应用配置文件
├── Program.cs               # 入口（Serilog 初始化 + 桌面启动）
└── App.axaml                # 全局资源 / DataTemplates 注册
```

## 数据模拟

项目内置 Mock 驱动层，无需真实硬件即可运行：

- **设备**：9 台设备（电机×2、水泵×2、阀门×2、传感器×2、储罐×1、输送带×1），分布于不同车间和单元
- **测点**：每台设备 2–4 个测点（温度、电流、转速、振动、流量、压力、液位、开度等），带上下限阈值
- **数据**：每秒自动生成含随机噪声的模拟数据；历史数据保留 1 小时内存缓存
- **报警**：每 5 秒有 30% 概率随机生成报警（Info/Warning/Critical），可实时查看与确认

## 系统架构

```
┌─────────────────────────────────────────────┐
│                 ViewModels                    │
│  Dashboard / DeviceList / Trends / AlarmList │
└──────────────────┬──────────────────────────┘
                   │ 调用
                   ▼
┌─────────────────────────────────────────────┐
│               ServiceLocator                  │
│         静态单例，桥接接口与实现                 │
└──────┬─────────────┬──────────────┬──────────┘
       │             │              │
       ▼             ▼              ▼
┌────────────┐ ┌──────────┐ ┌──────────────┐
│IDevice     │ │IData     │ │IAlarm        │
│Service     │ │Service   │ │Service       │
└──────┬─────┘ └─────┬────┘ └──────┬───────┘
       │              │             │
       └──────────────┼─────────────┘
                      ▼
          ┌──────────────────────┐
          │   Mock Drivers 层     │
          │ (可替换为真实 Modbus)  │
          └──────────────────────┘
```

## 快速开始

```bash
# 要求：.NET 10 SDK

# 克隆并运行
git clone <repo-url>
cd SimpleApp
dotnet restore
dotnet run
```

## 构建与发布

```bash
# 构建
dotnet build

# 发布（单文件自包含）
dotnet publish -c Release -r osx-x64 --self-contained true
dotnet publish -c Release -r osx-arm64 --self-contained true
dotnet publish -c Release -r win-x64 --self-contained true
dotnet publish -c Release -r linux-x64 --self-contained true

# 或使用发布脚本
bash publish.sh
```

项目同时配置了 GitHub Actions 自动构建，推送 `v*` 标签即可触发 Release。

## 未来扩展

- 替换 Mock 驱动为真实 Modbus TCP/RTU 通信协议
- 引入 SQLite/PostgreSQL 持久化历史数据
- 独立报警规则引擎，支持复杂条件与通知
- 用户认证与权限管理

## 许可证

MIT
