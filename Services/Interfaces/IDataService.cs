using SimpleApp.Core.Models;

namespace SimpleApp.Core.Services.Interfaces;

public interface IDataService
{
    Task<IEnumerable<DataPoint>> GetHistoricalDataAsync(string deviceId, DateTime from, DateTime to);
    Task<DataPoint?> GetLatestDataAsync(string deviceId);
    IObservable<DeviceData> DataStream { get; }
}
