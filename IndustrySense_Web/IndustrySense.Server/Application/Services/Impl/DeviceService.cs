using System.Linq.Expressions;
using System.Net;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Dao;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Application.Services.Impl
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceDao _deviceDao;

        public DeviceService(IDeviceDao deviceDao)
        {
            _deviceDao = deviceDao;
        }

        public void AddDevice(Device device)
        {
            _deviceDao.Insert(device);
        }

        public void RemoveDeviceById(int id)
        {
            _deviceDao.Delete(x => x.DeviceId == id);
        }

        public Device? GetDeviceByName(string name)
        {
            return _deviceDao.Select(x => x.DeviceName.Contains(name));
        }

        public Device? GetDeviceByIpAddress(string ip)
        {
            return _deviceDao.Select(x => x.DeviceIpAddress == ip);
        }

        public ResultSet<Device> GetDevices(int pageIndex)
        {
            return _deviceDao.SelectFilteredList(x => true, pageIndex);
        }

        public void UpdateDevice(int id, Device newDevice)
        {
            _deviceDao.Update(
                x => x.DeviceId == id,
                x =>
                {
                    x.DeviceId = newDevice.DeviceId;
                    x.DeviceName = newDevice.DeviceName;
                    x.DeviceIpAddress = newDevice.DeviceIpAddress;
                    x.ParsingRuleId = newDevice.ParsingRuleId;
                }
            );
        }

        public Device? GetDeviceById(int id)
        {
            return _deviceDao.Select(x => x.DeviceId == id);
        }
    }
}
