using System;
using System.Linq.Expressions;
using IndustrySense.Server.Dto;
using IndustrySense.Server.Infrastructure.Repository.Entity;
using IndustrySense.Server.Utilities;
using Microsoft.EntityFrameworkCore;

namespace IndustrySense.Server.Infrastructure.Repository.Dao.Impl
{
    public class DeviceDao : IDao<Device>
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public DeviceDao(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void Insert(Device entity)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                ctx.Devices.Add(entity);
                ctx.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Delete(Expression<Func<Device, bool>> filter)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                var entity = ctx.Devices.First(filter);
                ctx.Devices.Remove(entity);
                ctx.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Update(Expression<Func<Device, bool>> filter, Action<Device> predicate)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                var entity = ctx.Devices.First(filter);
                predicate.Invoke(entity);
                ctx.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Device? Select(Expression<Func<Device, bool>> filter)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            try
            {
                return ctx.Devices.FirstOrDefault(filter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResultSet<Device> SelectFilteredList(
            Expression<Func<Device, bool>> filter,
            int pageIndex
        )
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            try
            {
                return ctx.Devices.Where(filter).Page(pageIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
