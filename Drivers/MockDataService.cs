using System.Reactive.Subjects;
using SimpleApp.Core.Models;
using SimpleApp.Core.Services.Interfaces;
using Serilog;

namespace SimpleApp.Core.Drivers;

public class MockDataService : IDataService
{
    private readonly IDeviceService _deviceService;
    private readonly List<DataPoint> _historicalData;
    private readonly System.Timers.Timer _dataTimer;
    private readonly Subject<DeviceData> _dataSubject;

    public IObservable<DeviceData> DataStream => _dataSubject;

    public MockDataService(IDeviceService deviceService)
    {
        _deviceService = deviceService;
        _historicalData = new List<DataPoint>();
        _dataSubject = new Subject<DeviceData>();

        _dataTimer = new System.Timers.Timer(1000);
        _dataTimer.Elapsed += async (_, _) => await GenerateDataAsync();
        _dataTimer.Start();

        _ = PreloadHistoricalDataAsync();

        Log.Information("MockDataService started");
    }

    private async Task PreloadHistoricalDataAsync()
    {
        await Task.Delay(500);
        var devices = await _deviceService.GetAllDevicesAsync();
        var now = DateTime.Now;

        foreach (var device in devices)
        {
            foreach (var tag in device.Tags)
            {
                for (int i = 60; i >= 0; i--)
                {
                    var timestamp = now.AddMinutes(-i);
                    var baseValue = GetBaseValue(tag.Id);
                    var value = baseValue + new Random().NextDouble() * 10 - 5;

                    _historicalData.Add(new DataPoint
                    {
                        DeviceId = device.Id,
                        TagId = tag.Id,
                        Timestamp = timestamp,
                        Value = Math.Max(0, value)
                    });
                }
            }
        }

        Log.Information("Preloaded {Count} historical data points", _historicalData.Count);
    }

    private static double GetBaseValue(string tagId) => tagId switch
    {
        "TEMP" => 55,
        "CURRENT" => 25,
        "SPEED" => 1500,
        "VIBRATION" => 1.5,
        "FLOW" => 60,
        "PRESSURE" => 5,
        "LEVEL" => 65,
        "RUNTIME" => 5000,
        "POSITION" => 50,
        "LEAK" => 0,
        _ => 50
    };

    private async Task GenerateDataAsync()
    {
        try
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            foreach (var device in devices)
            {
                var data = await _deviceService.GetDeviceDataAsync(device.Id);
                if (data != null)
                {
                    foreach (var tagValue in data.Values)
                    {
                        _historicalData.Add(new DataPoint
                        {
                            DeviceId = device.Id,
                            TagId = tagValue.TagId,
                            Timestamp = data.Timestamp,
                            Value = tagValue.Value
                        });
                    }

                    _dataSubject.OnNext(data);
                }
            }

            var cutoff = DateTime.Now.AddHours(-1);
            _historicalData.RemoveAll(d => d.Timestamp < cutoff);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error generating mock data");
        }
    }

    public Task<IEnumerable<DataPoint>> GetHistoricalDataAsync(string deviceId, DateTime from, DateTime to)
    {
        var data = _historicalData
            .Where(d => d.DeviceId == deviceId && d.Timestamp >= from && d.Timestamp <= to)
            .OrderBy(d => d.Timestamp);
        return Task.FromResult(data.AsEnumerable());
    }

    public Task<DataPoint?> GetLatestDataAsync(string deviceId)
    {
        var latest = _historicalData
            .Where(d => d.DeviceId == deviceId)
            .OrderByDescending(d => d.Timestamp)
            .FirstOrDefault();
        return Task.FromResult(latest);
    }
}
