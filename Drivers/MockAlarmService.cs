using System.Reactive.Subjects;
using SimpleApp.Core.Models;
using SimpleApp.Core.Services.Interfaces;
using Serilog;

namespace SimpleApp.Core.Drivers;

public class MockAlarmService : IAlarmService
{
    private readonly List<Alarm> _alarms;
    private readonly Subject<Alarm> _alarmSubject;
    private readonly System.Timers.Timer _alarmTimer;
    private readonly Random _random;

    public IObservable<Alarm> AlarmStream => _alarmSubject;

    public MockAlarmService(IDeviceService deviceService)
    {
        _alarms = new List<Alarm>();
        _alarmSubject = new Subject<Alarm>();
        _random = new Random();

        GenerateInitialAlarms(deviceService);

        _alarmTimer = new System.Timers.Timer(5000);
        _alarmTimer.Elapsed += (_, _) => GenerateRandomAlarm(deviceService);
        _alarmTimer.Start();

        Log.Information("MockAlarmService started");
    }

    private void GenerateInitialAlarms(IDeviceService deviceService)
    {
        var devices = deviceService.GetAllDevicesAsync().Result.Take(3).ToList();
        var now = DateTime.Now;

        for (int i = 0; i < 5; i++)
        {
            var device = devices[i % devices.Count];
            var alarm = CreateAlarm(device, now.AddMinutes(-_random.Next(1, 60)));
            _alarms.Add(alarm);
        }
    }

    private void GenerateRandomAlarm(IDeviceService deviceService)
    {
        if (_random.NextDouble() > 0.3)
            return;

        var devices = deviceService.GetAllDevicesAsync().Result.ToList();
        var device = devices[_random.Next(devices.Count)];
        var alarm = CreateAlarm(device, DateTime.Now);
        _alarms.Add(alarm);
        _alarmSubject.OnNext(alarm);

        Log.Debug("Generated alarm: {AlarmMessage}", alarm.Message);
    }

    private Alarm CreateAlarm(Device device, DateTime timestamp)
    {
        var levels = new[] { AlarmLevel.Info, AlarmLevel.Warning, AlarmLevel.Critical };
        var types = new[] { AlarmType.High, AlarmType.HighHigh, AlarmType.Low, AlarmType.LowLow };
        var level = levels[_random.Next(levels.Length)];
        var type = types[_random.Next(types.Length)];

        var tag = device.Tags.FirstOrDefault();
        var value = tag != null ? GenerateAlarmValue(tag, level) : _random.NextDouble() * 100;
        var threshold = tag?.UpperLimit ?? 80;

        return new Alarm
        {
            Id = Guid.NewGuid().ToString("N")[..8],
            DeviceId = device.Id,
            TagId = tag?.Id ?? "UNKNOWN",
            Level = level,
            Type = type,
            Message = $"{device.Name} - {tag?.Name ?? "Unknown"} {GetAlarmTypeText(type)} ({value:F1} > {threshold:F1})",
            TriggerValue = value,
            Threshold = threshold,
            Timestamp = timestamp,
            IsAcknowledged = false
        };
    }

    private double GenerateAlarmValue(TagConfig tag, AlarmLevel level)
    {
        var upper = tag.UpperLimit ?? 100;
        var lower = tag.LowerLimit ?? 0;

        return level switch
        {
            AlarmLevel.Critical => upper + _random.NextDouble() * 10,
            AlarmLevel.Warning => upper * 0.9 + _random.NextDouble() * (upper * 0.1),
            AlarmLevel.Info => upper * 0.7 + _random.NextDouble() * (upper * 0.2),
            _ => upper
        };
    }

    private string GetAlarmTypeText(AlarmType type) => type switch
    {
        AlarmType.HighHigh => "高高报警",
        AlarmType.High => "高报警",
        AlarmType.Low => "低报警",
        AlarmType.LowLow => "低低报警",
        AlarmType.Deviation => "偏差报警",
        AlarmType.RateOfChange => "变化率报警",
        _ => "报警"
    };

    public Task<IEnumerable<Alarm>> GetAlarmsAsync(DateTime from, DateTime to)
    {
        var alarms = _alarms
            .Where(a => a.Timestamp >= from && a.Timestamp <= to)
            .OrderByDescending(a => a.Timestamp);
        return Task.FromResult(alarms.AsEnumerable());
    }

    public Task<bool> AcknowledgeAlarmAsync(string alarmId)
    {
        var alarm = _alarms.FirstOrDefault(a => a.Id == alarmId);
        if (alarm != null)
        {
            alarm.IsAcknowledged = true;
            alarm.AcknowledgedAt = DateTime.Now;
            alarm.AcknowledgedBy = "Operator";
            Log.Information("Alarm {AlarmId} acknowledged", alarmId);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
