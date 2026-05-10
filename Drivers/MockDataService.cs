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

        Log.Information("MockDataService started");
    }

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
