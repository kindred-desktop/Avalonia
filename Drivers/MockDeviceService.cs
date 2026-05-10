using SimpleApp.Core.Models;
using SimpleApp.Core.Services.Interfaces;
using Serilog;

namespace SimpleApp.Core.Drivers;

public class MockDeviceService : IDeviceService
{
    private readonly List<Device> _devices;
    private readonly Dictionary<string, DeviceStatus> _deviceStatuses;

    public MockDeviceService()
    {
        _devices = MockDeviceRegistry.GetDevices();
        _deviceStatuses = _devices.ToDictionary(d => d.Id, d => d.Status);
        Log.Information("MockDeviceService initialized with {Count} devices", _devices.Count);
    }

    public Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        foreach (var device in _devices)
        {
            device.Status = _deviceStatuses[device.Id];
            device.LastUpdate = DateTime.Now;
        }
        return Task.FromResult<IEnumerable<Device>>(_devices);
    }

    public Task<Device?> GetDeviceByIdAsync(string id)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device != null)
        {
            device.Status = _deviceStatuses[device.Id];
            device.LastUpdate = DateTime.Now;
        }
        return Task.FromResult(device);
    }

    public Task<DeviceData?> GetDeviceDataAsync(string deviceId)
    {
        var device = _devices.FirstOrDefault(d => d.Id == deviceId);
        if (device == null)
            return Task.FromResult<DeviceData?>(null);

        var data = GenerateDeviceData(device);
        return Task.FromResult<DeviceData?>(data);
    }

    public Task<bool> ConnectAsync(string deviceId)
    {
        if (_deviceStatuses.ContainsKey(deviceId))
        {
            _deviceStatuses[deviceId] = DeviceStatus.Online;
            Log.Information("Device {DeviceId} connected", deviceId);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> DisconnectAsync(string deviceId)
    {
        if (_deviceStatuses.ContainsKey(deviceId))
        {
            _deviceStatuses[deviceId] = DeviceStatus.Offline;
            Log.Information("Device {DeviceId} disconnected", deviceId);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public static DeviceData GenerateDeviceData(Device device)
    {
        var random = new Random();
        var data = new DeviceData
        {
            DeviceId = device.Id,
            Timestamp = DateTime.Now,
            Values = new List<TagValue>()
        };

        foreach (var tag in device.Tags)
        {
            var value = GenerateTagValue(tag, device.Type, random);
            data.Values.Add(value);
        }

        return data;
    }

    private static TagValue GenerateTagValue(TagConfig tag, DeviceType deviceType, Random random)
    {
        double baseValue = tag.Id switch
        {
            "TEMP" => 45 + random.NextDouble() * 30,
            "CURRENT" => 10 + random.NextDouble() * 30,
            "SPEED" => 1400 + random.NextDouble() * 200,
            "VIBRATION" => 0.5 + random.NextDouble() * 2,
            "FLOW" => 40 + random.NextDouble() * 40,
            "PRESSURE" => 3 + random.NextDouble() * 5,
            "LEVEL" => 30 + random.NextDouble() * 50,
            "RUNTIME" => random.NextDouble() * 10000,
            "POSITION" => random.NextDouble() * 100,
            "LEAK" => random.NextDouble() > 0.98 ? 1 : 0,
            "LOAD" => 30 + random.NextDouble() * 50,
            _ => random.NextDouble() * 100
        };

        double noise = (random.NextDouble() - 0.5) * 2;
        if (tag.Id == "VIBRATION" || tag.Id == "TEMP")
            noise *= 0.5;

        return new TagValue
        {
            TagId = tag.Id,
            Value = Math.Round(baseValue + noise, 2),
            IsValid = true
        };
    }
}
