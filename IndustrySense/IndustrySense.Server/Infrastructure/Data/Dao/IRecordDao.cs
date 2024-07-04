using System.Linq.Expressions;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Infrastructure.Data.Dao
{
    public interface IRecordDao
    {
        void Delete(Expression<Func<Record, bool>> filter);
        void Insert(Record entity);
        Record? Select(Expression<Func<Record, bool>> filter);
        ResultSet<Record> SelectFilteredList(Expression<Func<Record, bool>> filter, int pageIndex);
        void Update(Expression<Func<Record, bool>> filter, Action<Record> predicate);
    }
}
