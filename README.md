# SimpleApp - 工业自动控制系统

基于 Avalonia UI 和 .NET 10 的工业自动化监控应用程序。

## 功能特性

- 📊 **仪表盘** - 系统概览、统计指标、报警概览
- 🔧 **设备列表** - 设备管理、状态监控、搜索过滤
- 📈 **趋势数据** - 实时数据曲线、历史趋势
- 🔔 **报警列表** - 报警管理、确认处理、等级筛选
- ⚙️ **系统设置** - Modbus 配置、轮询间隔、告警设置

## 技术栈

- **框架**: Avalonia UI 12.0.2
- **运行时**: .NET 10.0
- **架构**: MVVM (CommunityToolkit.Mvvm 8.4.0)
- **通信**: Modbus TCP/RTU (Mock 数据模拟)
- **日志**: Serilog

## 项目结构

```
SimpleApp/
├── Models/              # 数据模型
├── ViewModels/          # 视图模型
├── Views/               # 视图
├── Services/            # 服务接口
├── Drivers/             # 驱动程序 (Mock)
├── Converters/          # 值转换器
├── App.axaml            # 应用配置
└── MainWindow.axaml     # 主窗口
```

## 运行

```bash
dotnet restore
dotnet run
```

## 许可证

MIT
