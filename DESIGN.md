# SimpleApp 工业自动控制系统 - 设计文档

## 1. 项目概述

SimpleApp 是一个基于 Avalonia UI 的工业自动化控制系统，模拟 SCADA（数据采集与监控系统）功能。系统支持设备监控、数据采集、报警管理和趋势分析。

## 2. 技术栈

| 组件 | 技术 | 版本 |
|------|------|------|
| UI 框架 | Avalonia UI | 12.0.2 |
| 运行时 | .NET | 10.0 |
| 架构模式 | MVVM | CommunityToolkit.Mvvm 8.4.0 |
| 响应式编程 | System.Reactive | 6.0.1 |
| 日志框架 | Serilog | 4.0.0 |
| 日志输出 | Serilog.Sinks.File | 6.0.0 |

## 3. 项目架构

```
SimpleApp/
├── Models/                    # 数据模型层
│   ├── Device.cs             # 设备模型
│   ├── DeviceData.cs         # 设备实时数据
│   ├── DataPoint.cs          # 数据点
│   ├── TagConfig.cs          # 测点配置
│   ├── TagValue.cs           # 测点值
│   ├── Alarm.cs              # 报警模型
│   ├── AlarmEnums.cs         # 报警枚举
│   ├── DeviceStatus.cs       # 设备状态枚举
│   ├── DeviceType.cs         # 设备类型枚举
│   └── TagValue.cs           # 测点值
│
├── ViewModels/               # 视图模型层 (MVVM)
│   ├── ViewModelBase.cs      # ViewModel 基类
│   ├── MainWindowViewModel.cs # 主窗口 VM
│   ├── DashboardViewModel.cs  # 仪表盘 VM
│   ├── DeviceListViewModel.cs # 设备列表 VM
│   ├── TrendsViewModel.cs     # 趋势图 VM
│   ├── AlarmListViewModel.cs  # 报警列表 VM
│   └── SettingsViewModel.cs  # 设置 VM
│
├── Views/                    # 视图层 (XAML)
│   ├── MainWindow.axaml      # 主窗口
│   ├── DashboardView.axaml   # 仪表盘视图
│   ├── DeviceListView.axaml  # 设备列表视图
│   ├── TrendsView.axaml      # 趋势图视图
│   ├── AlarmListView.axaml   # 报警列表视图
│   └── SettingsView.axaml    # 设置视图
│
├── Services/                 # 服务接口层
│   └── Interfaces/
│       ├── IDeviceService.cs # 设备服务接口
│       ├── IDataService.cs    # 数据服务接口
│       └── IAlarmService.cs   # 报警服务接口
│
├── Drivers/                  # 驱动程序层 (Mock 实现)
│   ├── MockDeviceRegistry.cs # 设备注册表
│   ├── MockDeviceService.cs  # 设备服务 Mock
│   ├── MockDataService.cs    # 数据服务 Mock
│   └── MockAlarmService.cs   # 报警服务 Mock
│
├── Converters/              # 值转换器
│   └── Converters.cs         # XAML 数据绑定转换器
│
├── App.axaml                # 应用配置
├── Program.cs               # 程序入口
└── app.manifest            # 应用清单
```

## 4. 核心模块设计

### 4.1 数据模型 (Models)

#### 设备模型 (Device)
```csharp
public class Device
{
    public string Id { get; set; }          // 设备唯一标识
    public string Name { get; set; }        // 设备名称
    public string Code { get; set; }         // 设备编码
    public DeviceType Type { get; set; }     // 设备类型 (Motor/Pump/Valve/Sensor)
    public DeviceStatus Status { get; set; }  // 设备状态 (Running/Idle/Offline/Fault)
    public string Location { get; set; }     // 安装位置
    public string Unit { get; set; }         // 所属单元
    public DateTime LastUpdate { get; set; } // 最后更新时间
    public List<TagConfig> Tags { get; set; } // 关联测点列表
}
```

#### 数据点 (DataPoint)
```csharp
public class DataPoint
{
    public string DeviceId { get; set; }    // 设备ID
    public string TagId { get; set; }       // 测点ID
    public DateTime Timestamp { get; set; }  // 时间戳
    public double Value { get; set; }        // 数值
}
```

#### 报警模型 (Alarm)
```csharp
public class Alarm
{
    public string Id { get; set; }
    public string DeviceId { get; set; }    // 触发设备
    public string TagId { get; set; }       // 触发测点
    public AlarmLevel Level { get; set; }   // 报警等级 (Info/Warning/Error/Critical)
    public string Message { get; set; }     // 报警信息
    public double TriggerValue { get; set; } // 触发值
    public double Threshold { get; set; }   // 阈值
    public DateTime Timestamp { get; set; } // 发生时间
    public bool IsAcknowledged { get; set; } // 是否已确认
}
```

### 4.2 服务接口 (Services/Interfaces)

```
┌─────────────────────────────────────────────────────────────┐
│                      ViewModels                             │
│  DashboardViewModel, DeviceListViewModel, TrendsViewModel  │
└─────────────────────┬───────────────────────────────────────┘
                      │ 调用
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                    ServiceLocator                            │
│              (服务定位器，单例模式)                           │
└─────────────────────┬───────────────────────────────────────┘
                      │ 依赖注入
         ┌────────────┼────────────┐
         ▼            ▼            ▼
   ┌──────────┐ ┌──────────┐ ┌──────────┐
   │IDevice   │ │IData     │ │IAlarm    │
   │Service   │ │Service   │ │Service   │
   └───┬──────┘ └────┬────┘ └────┬────┘
       │              │            │
       └──────────────┼────────────┘
                      │ 实现
                      ▼
              ┌──────────────┐
              │Mock Drivers  │
              │(模拟驱动层)   │
              └──────────────┘
```

#### 服务接口定义

```csharp
// IDeviceService - 设备管理
public interface IDeviceService
{
    Task<IEnumerable<Device>> GetAllDevicesAsync();
    Task<Device?> GetDeviceAsync(string deviceId);
    Task<DeviceData?> GetDeviceDataAsync(string deviceId);
    Task<bool> ConnectAsync(string deviceId);
    Task<bool> DisconnectAsync(string deviceId);
}

// IDataService - 数据采集
public interface IDataService
{
    IObservable<DeviceData> DataStream { get; }  // 实时数据流
    Task<IEnumerable<DataPoint>> GetHistoricalDataAsync(string deviceId, DateTime from, DateTime to);
    Task<DataPoint?> GetLatestDataAsync(string deviceId);
}

// IAlarmService - 报警管理
public interface IAlarmService
{
    IObservable<Alarm> AlarmStream { get; }     // 报警流
    Task<IEnumerable<Alarm>> GetAlarmsAsync(DateTime from, DateTime to);
    Task<bool> AcknowledgeAlarmAsync(string alarmId);
    Task AcknowledgeAllAsync();
}
```

### 4.3 驱动程序 (Mock Drivers)

Mock 驱动层模拟真实的工业设备通信协议（Modbus TCP/RTU）。

#### MockDeviceRegistry
设备注册表，预定义 6 种工业设备：
- **MOT-001/MOT-002**: 原料泵电机 / 成品泵电机
- **PMP-001/PMP-002**: 循环水泵 / 冷却水泵
- **VLV-001**: 进料阀
- **SNS-001**: 温度传感器

每种设备配置不同的测点：
```
电机: 温度(TEMP), 电流(CURRENT), 转速(SPEED), 振动(VIBRATION)
泵:  流量(FLOW), 压力(PRESSURE), 液位(LEVEL), 运行时间(RUNTIME)
阀:  开度(POSITION), 压力(PRESSURE), 温度(TEMP), 泄漏检测(LEAK)
传感器: 温度(TEMP), 湿度(HUMIDITY), 压力(PRESSURE)
```

#### MockDataService
数据服务 Mock，负责：
1. **定时数据生成**: 每秒生成所有设备的实时数据
2. **历史数据缓存**: 内存中维护 1 小时历史数据
3. **初始数据预加载**: 启动时预生成 60 分钟历史数据
4. **数据流发布**: 通过 Reactive Subject 发布实时数据

```csharp
public class MockDataService : IDataService
{
    // 每秒生成数据
    private readonly System.Timers.Timer _dataTimer;
    private readonly Subject<DeviceData> _dataSubject;

    // 预加载历史数据
    private async Task PreloadHistoricalDataAsync()
    {
        // 为每个设备的每个测点生成60分钟历史数据
    }

    // 实时数据流
    public IObservable<DeviceData> DataStream => _dataSubject;
}
```

## 5. 功能页面设计

### 5.1 仪表盘 (DashboardView)

**功能**: 系统总览，展示关键指标和状态。

**布局**:
```
┌─────────────────────────────────────────────────────────────┐
│  仪表盘                                        2024-01-15  │
├─────────┬─────────┬─────────┬─────────┐                   │
│ 📊      │ 🟢      │ ⚙️      │ ⚠️      │                   │
│ 总设备数 │ 在线设备 │ 运行中   │ 告警/故障 │                   │
│    8    │    6    │    4    │    2    │                   │
└─────────┴─────────┴─────────┴─────────┘                   │
┌─────────────────────┐ ┌─────────────────────┐              │
│     报警概览          │ │     最近告警          │              │
│ ┌─────┐ ┌─────┐     │ │ ⚠️ 温度过高 - MOT-001 │
│ │活动 │ │严重 │     │ │ ⚠️ 振动异常 - PMP-001 │
│ │ 3   │ │ 1   │     │ │ 🔶 电流波动 - MOT-002 │
│ └─────┘ └─────┘     │ │                      │
└─────────────────────┘ └─────────────────────┘              │
┌─────────────────────────────────────────────────────────────┐
│                      设备状态分布                            │
│  正常运行(4)  待机(2)  故障(1)  离线(1)                      │
└─────────────────────────────────────────────────────────────┘
```

**ViewModel 职责**:
- `TotalDevices`: 设备总数
- `OnlineDevices`: 在线设备数
- `RunningDevices`: 运行中设备数
- `WarningDevices`: 告警/故障设备数
- `ActiveAlarms`: 活动报警数
- `CriticalAlarms`: 严重报警数
- `RecentAlarms`: 最近报警列表（最近5条）

### 5.2 设备列表 (DeviceListView)

**功能**: 设备管理，支持搜索和过滤。

**布局**:
```
┌─────────────────────────────────────────────────────────────┐
│  设备列表                              [5 台设备]            │
├─────────────────────────────────────────────────────────────┤
│ [搜索设备...] [类型▼] [状态▼] [刷新]                          │
├─────────────────────────────────────────────────────────────┤
│ ┌───────────────────┐ ┌───────────────────┐ ┌────────────┐ │
│ │ 🔧 原料泵电机      │ │ 🔧 成品泵电机      │ │ 🔧 循环水泵│ │
│ │    MOT-001        │ │    MOT-002        │ │   PMP-001  │ │
│ ├───────────────────┤ ├───────────────────┤ ├────────────┤ │
│ │ 位置: 车间A-区域1 │ │ 位置: 车间A-区域2 │ │ 位置:...   │ │
│ │ 单位: 原料处理单元 │ │ 单位: 成品处理单元 │ │ 单位:...   │ │
│ │ ┌状态: 运行中┐    │ │ ┌状态: 运行中┐    │ │ ┌状态...┐ │ │
│ │ └绿色标签┘  │    │ │ └绿色标签┘  │    │ │ └标签┘  │ │
│ ├───────────────────┤ ├───────────────────┤ ├────────────┤ │
│ │   [查看详情]       │ │   [查看详情]       │ │ [查看详情] │ │
│ └───────────────────┘ └───────────────────┘ └────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

**ViewModel 职责**:
- `AvailableDevices`: 所有设备列表
- `FilteredDevices`: 过滤后的设备列表（根据搜索和筛选条件）
- `SearchText`: 搜索文本
- `SelectedTypeFilter`: 选中的类型过滤器
- `SelectedStatusFilter`: 选中的状态过滤器
- `TypeFilters`: 类型过滤器选项（全部/电机/泵/阀/传感器）
- `StatusFilters`: 状态过滤器选项（全部/运行/待机/离线/故障）

### 5.3 趋势数据 (TrendsView)

**功能**: 查看设备测点的历史趋势和实时数据。

**布局**:
```
┌─────────────────────────────────────────────────────────────┐
│  趋势图                                                        │
│ [选择设备▼] [选择测点▼] [1小时▼] [刷新]                         │
├─────────────────────────────────────────────────────────────┤
│  趋势曲线                                                       │
│ 100 ┐                           ┌─────────────────────────  │
│     │                    ┌─────┐│                            │
│  50 ┤              ┌─────┤     ││                            │
│     │        ┌─────┤     │     ││                            │
│   0 ┼────────┘     │     │     │┼────────────────────────   │
│     00:00    04:00  08:00 12:00 16:00 20:00                   │
├─────────────────────────────────────────────────────────────┤
│  实时数据                                     [滚动查看]        │
│ ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐             │
│ │  TEMP   │ │ CURRENT │ │  SPEED  │ │VIBRATION│             │
│ │  65.32  │ │  28.45  │ │ 1523.8  │ │   1.23  │             │
│ │ 14:32:15│ │ 14:32:15│ │ 14:32:15│ │ 14:32:15│             │
│ └─────────┘ └─────────┘ └─────────┘ └─────────┘             │
└─────────────────────────────────────────────────────────────┘
```

**ViewModel 职责**:
- `AvailableDevices`: 可选设备列表
- `SelectedDevice`: 选中的设备
- `AvailableTags`: 当前设备的测点列表
- `SelectedTag`: 选中的测点
- `TimeRanges`: 时间范围选项（实时/1小时/6小时/24小时/7天）
- `SelectedTimeRange`: 选中的时间范围
- `ChartData`: 图表数据点列表

**数据流程**:
```
用户选择设备/测点/时间范围
         │
         ▼
LoadDataAsync()
         │
         ├── IDataService.GetHistoricalDataAsync(deviceId, from, to)
         │            │
         │            ▼
         │    MockDataService 返回过滤后的历史数据
         │
         ▼
ChartData 更新 → UI 自动刷新
```

### 5.4 报警列表 (AlarmListView)

**功能**: 查看和管理系统报警。

**布局**:
```
┌─────────────────────────────────────────────────────────────┐
│  报警列表                                    [8 条报警]       │
│ [等级▼] [状态▼] [✓ 确认全部] [🔄 刷新]                        │
├─────────────────────────────────────────────────────────────┤
│ ┌────┬─────────────────────────────────────────┬────────┬──┐│
│ │    │ 🔥 温度超过上限 80°C                      │ 未确认 │  ││
│ │ ■  │ 设备: MOT-001  测点: TEMP  触发值: 85.2  │ [确认] │  ││
│ │    │ 2024-01-15 14:32:15                     │        │  ││
│ ├────┼─────────────────────────────────────────┼────────┼──┤│
│ │ ▣  │ ⚠️ 振动异常                              │ 已确认 │  ││
│ │    │ 设备: PMP-001  测点: VIBRATION          │ [确认] │  ││
│ │    │ 2024-01-15 14:30:00                     │        │  ││
│ └────┴─────────────────────────────────────────┴────────┴──┘│
└─────────────────────────────────────────────────────────────┘
```

**ViewModel 职责**:
- `Alarms`: 报警列表
- `LevelFilters`: 等级过滤器（全部/信息/警告/错误/严重）
- `StatusFilters`: 状态过滤器（全部/未确认/已确认）
- `SelectedLevelFilter`: 选中的等级过滤
- `SelectedStatusFilter`: 选中的状态过滤
- `AcknowledgeAlarmCommand`: 确认单条报警
- `AcknowledgeAllCommand`: 确认全部报警
- `RefreshCommand`: 刷新报警列表

### 5.5 设置 (SettingsView)

**功能**: 系统配置参数。

**布局**:
```
┌─────────────────────────────────────────────────────────────┐
│  设置                                                           │
├─────────────────────────────────────────────────────────────┤
│  Modbus 配置                                                    │
│  ┌───────────────────────────────────────────────────────┐   │
│  │ IP 地址:    [192.168.1.100                      ]     │   │
│  │ 端口:      [502                             ]         │   │
│  │ 轮询间隔:  [1000] ms                            ]     │   │
│  └───────────────────────────────────────────────────────┘   │
│                                                                 │
│  数据存储                                                        │
│  ┌───────────────────────────────────────────────────────┐   │
│  │ 历史数据保留: [30] 天                                 │   │
│  └───────────────────────────────────────────────────────┘   │
│                                                                 │
│  告警设置                                                        │
│  ┌───────────────────────────────────────────────────────┐   │
│  │ ☑ 启用报警声音                                        │   │
│  │ 声音文件: [default.wav                         ]     │   │
│  └───────────────────────────────────────────────────────┘   │
│                                                                 │
│                              [保存设置]                         │
└─────────────────────────────────────────────────────────────┘
```

**ViewModel 职责**:
- `ModbusIp`: Modbus IP 地址
- `ModbusPort`: Modbus 端口
- `PollingInterval`: 轮询间隔（毫秒）
- `HistoryRetentionDays`: 历史数据保留天数
- `EnableAlarmSound`: 是否启用报警声音
- `AlarmSoundFile`: 报警声音文件
- `SaveSettingsCommand`: 保存设置命令

## 6. 数据流设计

### 6.1 实时数据流

```
┌─────────────────┐     每秒      ┌─────────────────┐     发布      ┌─────────────────┐
│ MockDataService │ ─────────▶ │   Subject<>     │ ─────────▶ │   订阅者         │
│ (定时器)         │   生成数据   │   (数据流)       │   数据      │ (Dashboard/其他) │
└─────────────────┘            └─────────────────┘            └─────────────────┘
```

### 6.2 报警检测流

```
设备数据更新
      │
      ▼
┌─────────────────┐
│ 超过阈值检测     │
│ (温度 > 80°C)   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ AlarmService    │ ────── 发布 ──────▶ 报警列表更新
│ (报警生成)       │
└─────────────────┘
```

## 7. ViewModel 基类设计

```csharp
public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;
}
```

使用 CommunityToolkit.Mvvm 的 source generator：
- `[ObservableProperty]` 自动生成属性和 OnPropertyChanged
- `[RelayCommand]` 自动生成 ICommand

## 8. XAML 数据绑定

### 8.1 Application.DataTemplates (App.axaml)

```xml
<Application.DataTemplates>
    <DataTemplate DataType="vm:DashboardViewModel">
        <views:DashboardView/>
    </DataTemplate>
    <DataTemplate DataType="vm:DeviceListViewModel">
        <views:DeviceListView/>
    </DataTemplate>
    <!-- ... 其他视图 -->
</Application.DataTemplates>
```

Avalonia 根据 DataType 自动匹配 ViewModel 到对应的 View。

### 8.2 编译时绑定 (x:DataType)

```xml
<UserControl x:Class="SimpleApp.Desktop.Views.DashboardView"
             x:DataType="vm:DashboardViewModel">
    <TextBlock Text="{Binding Title}"/>
</UserControl>
```

`x:DataType` 启用编译时绑定检查，提高性能并减少运行时错误。

### 8.3 值转换器 (Converters)

```csharp
public class AlarmLevelToColorConverter : IValueConverter
{
    public object? Convert(object? value, ...)
    {
        return value?.ToString() switch
        {
            "Critical" => new SolidColorBrush(Color.Parse("#EF4444")),
            "Warning" => new SolidColorBrush(Color.Parse("#F59E0B")),
            "Info" => new SolidColorBrush(Color.Parse("#3B82F6")),
            _ => new SolidColorBrush(Color.Parse("#94A3B8"))
        };
    }
}
```

## 9. 服务定位器 (ServiceLocator)

```csharp
public static class ServiceLocator
{
    public static IDeviceService DeviceService { get; private set; }
    public static IDataService DataService { get; private set; }
    public static IAlarmService AlarmService { get; private set; }

    public static void Initialize()
    {
        var deviceService = new MockDeviceService();
        DeviceService = deviceService;
        DataService = new MockDataService(deviceService);
        AlarmService = new MockAlarmService(deviceService, DataService);
    }
}
```

## 10. 运行与构建

### 运行
```bash
cd SimpleApp
dotnet restore
dotnet run
```

### 构建
```bash
dotnet build
```

### 发布
```bash
dotnet publish -c Release -r osx-x64
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r linux-x64
```

## 11. 未来扩展

### 11.1 真实 Modbus 通信
当前 Mock 驱动可替换为真实驱动：
```csharp
public class ModbusDeviceService : IDeviceService
{
    private readonly ModbusTcpClient _client;

    public async Task<DeviceData?> GetDeviceDataAsync(string deviceId)
    {
        // 读取 Modbus 寄存器
        var registers = await _client.ReadHoldingRegistersAsync(...);
        return ParseRegisters(registers);
    }
}
```

### 11.2 数据持久化
- 当前数据存储在内存中
- 可扩展支持 SQLite/PostgreSQL 存储历史数据

### 11.3 真实报警引擎
- 当前阈值检测在 MockDriver 中实现
- 可分离为独立的报警规则引擎，支持复杂条件

## 12. 许可证

MIT License
