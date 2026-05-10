using SimpleApp.Core.Models;

namespace SimpleApp.Core.Services.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<Device>> GetAllDevicesAsync();
    Task<Device?> GetDeviceByIdAsync(string id);
    Task<DeviceData?> GetDeviceDataAsync(string deviceId);
    Task<bool> ConnectAsync(string deviceId);
    Task<bool> DisconnectAsync(string deviceId);
}
