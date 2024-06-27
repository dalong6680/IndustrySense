using IndustrySense.Server.Dto;
using System;
using System.Linq.Expressions;

namespace IndustrySense.Server.Infrastructure.Repository.Dao
{
    public interface IDao<T>
        where T : class
    {
        void Insert(T entity);
        void Delete(Expression<Func<T, bool>> filter);
        void Update(Expression<Func<T, bool>> filter, Action<T> predicate);
        T? Select(Expression<Func<T, bool>> filter);
        ResultSet<T> SelectFilteredList(Expression<Func<T, bool>> filter, int pageIndex);
    }
}
