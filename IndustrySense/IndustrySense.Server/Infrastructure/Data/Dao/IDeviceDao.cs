using System.Linq.Expressions;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Infrastructure.Data.Dao
{
    public interface IDeviceDao
    {
        void Delete(Expression<Func<Device, bool>> filter);
        void Insert(Device entity);
        Device? Select(Expression<Func<Device, bool>> filter);
        ResultSet<Device> SelectFilteredList(Expression<Func<Device, bool>> filter, int pageIndex);
        void Update(Expression<Func<Device, bool>> filter, Action<Device> predicate);
    }
}
