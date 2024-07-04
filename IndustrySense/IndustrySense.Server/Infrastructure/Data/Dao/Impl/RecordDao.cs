using System;
using System.Linq;
using System.Linq.Expressions;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Common.Helpers;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace IndustrySense.Server.Infrastructure.Data.Dao.Impl
{
    public class RecordDao : IRecordDao
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public RecordDao(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void Insert(Record entity)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                ctx.Record.Add(entity);
                ctx.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Delete(Expression<Func<Record, bool>> filter)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                var entity = ctx.Record.First(filter);
                ctx.Record.Remove(entity);
                ctx.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Update(Expression<Func<Record, bool>> filter, Action<Record> predicate)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                var entity = ctx.Record.First(filter);
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

        public Record? Select(Expression<Func<Record, bool>> filter)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            try
            {
                return ctx.Record.FirstOrDefault(filter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResultSet<Record> SelectFilteredList(
            Expression<Func<Record, bool>> filter,
            int pageIndex
        )
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            try
            {
                return ctx.Record.Where(filter).Page(pageIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
