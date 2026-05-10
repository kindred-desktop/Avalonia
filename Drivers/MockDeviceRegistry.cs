using SimpleApp.Core.Models;

namespace SimpleApp.Core.Drivers;

public static class MockDeviceRegistry
{
    public static List<Device> GetDevices()
    {
        return new List<Device>
        {
            new Device
            {
                Id = "MOT-001",
                Name = "原料泵电机",
                Code = "MOT-001",
                Type = DeviceType.Motor,
                Status = DeviceStatus.Running,
                Location = "车间A-区域1",
                Unit = "原料处理单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "TEMP", Name = "温度", Unit = "°C", UpperLimit = 80, LowerLimit = 20 },
                    new() { Id = "CURRENT", Name = "电流", Unit = "A", UpperLimit = 50 },
                    new() { Id = "SPEED", Name = "转速", Unit = "rpm", UpperLimit = 3000 },
                    new() { Id = "VIBRATION", Name = "振动", Unit = "mm/s", UpperLimit = 4.5 }
                }
            },
            new Device
            {
                Id = "MOT-002",
                Name = "成品泵电机",
                Code = "MOT-002",
                Type = DeviceType.Motor,
                Status = DeviceStatus.Running,
                Location = "车间A-区域2",
                Unit = "成品处理单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "TEMP", Name = "温度", Unit = "°C", UpperLimit = 80, LowerLimit = 20 },
                    new() { Id = "CURRENT", Name = "电流", Unit = "A", UpperLimit = 50 },
                    new() { Id = "SPEED", Name = "转速", Unit = "rpm", UpperLimit = 3000 },
                    new() { Id = "VIBRATION", Name = "振动", Unit = "mm/s", UpperLimit = 4.5 }
                }
            },
            new Device
            {
                Id = "PMP-001",
                Name = "循环水泵",
                Code = "PMP-001",
                Type = DeviceType.Pump,
                Status = DeviceStatus.Running,
                Location = "车间B-区域1",
                Unit = "水处理单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "FLOW", Name = "流量", Unit = "m³/h", UpperLimit = 100 },
                    new() { Id = "PRESSURE", Name = "压力", Unit = "bar", UpperLimit = 10 },
                    new() { Id = "LEVEL", Name = "液位", Unit = "%", UpperLimit = 95, LowerLimit = 10 },
                    new() { Id = "RUNTIME", Name = "运行时间", Unit = "h" }
                }
            },
            new Device
            {
                Id = "PMP-002",
                Name = "冷却水泵",
                Code = "PMP-002",
                Type = DeviceType.Pump,
                Status = DeviceStatus.Idle,
                Location = "车间B-区域2",
                Unit = "水处理单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "FLOW", Name = "流量", Unit = "m³/h", UpperLimit = 100 },
                    new() { Id = "PRESSURE", Name = "压力", Unit = "bar", UpperLimit = 10 },
                    new() { Id = "LEVEL", Name = "液位", Unit = "%", UpperLimit = 95, LowerLimit = 10 },
                    new() { Id = "RUNTIME", Name = "运行时间", Unit = "h" }
                }
            },
            new Device
            {
                Id = "VLV-001",
                Name = "进料阀",
                Code = "VLV-001",
                Type = DeviceType.Valve,
                Status = DeviceStatus.Online,
                Location = "车间A-反应器",
                Unit = "反应单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "POSITION", Name = "开度", Unit = "%", UpperLimit = 100 },
                    new() { Id = "PRESSURE", Name = "压力", Unit = "bar", UpperLimit = 10 },
                    new() { Id = "TEMP", Name = "温度", Unit = "°C", UpperLimit = 100 },
                    new() { Id = "LEAK", Name = "泄漏检测", Unit = "" }
                }
            },
            new Device
            {
                Id = "VLV-002",
                Name = "排气阀",
                Code = "VLV-002",
                Type = DeviceType.Valve,
                Status = DeviceStatus.Online,
                Location = "车间A-反应器",
                Unit = "反应单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "POSITION", Name = "开度", Unit = "%", UpperLimit = 100 },
                    new() { Id = "PRESSURE", Name = "压力", Unit = "bar", UpperLimit = 10 },
                    new() { Id = "TEMP", Name = "温度", Unit = "°C", UpperLimit = 100 },
                    new() { Id = "LEAK", Name = "泄漏检测", Unit = "" }
                }
            },
            new Device
            {
                Id = "SN-001",
                Name = "反应器温度",
                Code = "SN-001",
                Type = DeviceType.Sensor,
                Status = DeviceStatus.Online,
                Location = "车间A-反应器",
                Unit = "反应单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "TEMP", Name = "温度", Unit = "°C", UpperLimit = 90, LowerLimit = 20 },
                    new() { Id = "PRESSURE", Name = "压力", Unit = "bar", UpperLimit = 8 }
                }
            },
            new Device
            {
                Id = "SN-002",
                Name = "储罐液位",
                Code = "SN-002",
                Type = DeviceType.Sensor,
                Status = DeviceStatus.Online,
                Location = "车间B-储罐",
                Unit = "储罐单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "LEVEL", Name = "液位", Unit = "%", UpperLimit = 95, LowerLimit = 5 },
                    new() { Id = "TEMP", Name = "温度", Unit = "°C", UpperLimit = 60 }
                }
            },
            new Device
            {
                Id = "TNK-001",
                Name = "原料储罐",
                Code = "TNK-001",
                Type = DeviceType.Tank,
                Status = DeviceStatus.Running,
                Location = "车间B-储罐区",
                Unit = "储罐单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "LEVEL", Name = "液位", Unit = "%", UpperLimit = 95, LowerLimit = 10 },
                    new() { Id = "TEMP", Name = "温度", Unit = "°C", UpperLimit = 60 },
                    new() { Id = "PRESSURE", Name = "压力", Unit = "bar", UpperLimit = 5 }
                }
            },
            new Device
            {
                Id = "CNV-001",
                Name = "输送带1",
                Code = "CNV-001",
                Type = DeviceType.Conveyor,
                Status = DeviceStatus.Running,
                Location = "车间C-包装线",
                Unit = "包装单元",
                LastUpdate = DateTime.Now,
                Tags = new List<TagConfig>
                {
                    new() { Id = "SPEED", Name = "速度", Unit = "m/min", UpperLimit = 50 },
                    new() { Id = "LOAD", Name = "负载", Unit = "%", UpperLimit = 90 },
                    new() { Id = "RUNTIME", Name = "运行时间", Unit = "h" }
                }
            }
        };
    }
}
