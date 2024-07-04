using System;
using System.Linq.Expressions;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Common.Helpers;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace IndustrySense.Server.Infrastructure.Data.Dao.Impl
{
    public class DeviceDao : IDeviceDao
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
                ctx.Device.Add(entity);
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
                var entity = ctx.Device.First(filter);
                ctx.Device.Remove(entity);
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
                var entity = ctx.Device.First(filter);
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
                return ctx.Device.FirstOrDefault(filter);
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
                return ctx.Device.Where(filter).Page(pageIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
