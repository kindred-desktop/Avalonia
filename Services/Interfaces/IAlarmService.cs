using SimpleApp.Core.Models;

namespace SimpleApp.Core.Services.Interfaces;

public interface IAlarmService
{
    Task<IEnumerable<Alarm>> GetAlarmsAsync(DateTime from, DateTime to);
    Task<bool> AcknowledgeAlarmAsync(string alarmId);
    IObservable<Alarm> AlarmStream { get; }
}
