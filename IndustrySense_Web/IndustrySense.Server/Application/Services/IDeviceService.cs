using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Application.Services
{
    public interface IDeviceService
    {
        void AddDevice(Device device);
        Device? GetDeviceByIpAddress(string ip);
        Device? GetDeviceByName(string name);
        Device? GetDeviceById(int id);
        ResultSet<Device> GetDevices(int pageIndex);
        void RemoveDeviceById(int id);
        void UpdateDevice(int id, Device newDevice);
    }
}
