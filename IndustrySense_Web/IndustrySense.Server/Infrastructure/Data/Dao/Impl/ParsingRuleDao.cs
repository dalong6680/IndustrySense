using System;
using System.Linq;
using System.Linq.Expressions;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Common.Helpers;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace IndustrySense.Server.Infrastructure.Data.Dao.Impl
{
    public class ParsingRuleDao : IParsingRuleDao
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public ParsingRuleDao(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public void Insert(ParsingRule entity)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                ctx.ParsingRule.Add(entity);
                ctx.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Delete(Expression<Func<ParsingRule, bool>> filter)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                var entity = ctx.ParsingRule.First(filter);
                ctx.ParsingRule.Remove(entity);
                ctx.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Update(
            Expression<Func<ParsingRule, bool>> filter,
            Action<ParsingRule> predicate
        )
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            using var transaction = ctx.Database.BeginTransaction();
            try
            {
                var entity = ctx.ParsingRule.First(filter);
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

        public ParsingRule? Select(Expression<Func<ParsingRule, bool>> filter)
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            try
            {
                return ctx.ParsingRule.FirstOrDefault(filter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ResultSet<ParsingRule> SelectFilteredList(
            Expression<Func<ParsingRule, bool>> filter,
            int pageIndex
        )
        {
            using var ctx = _dbContextFactory.CreateDbContext();
            try
            {
                return ctx.ParsingRule.Where(filter).Page(pageIndex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
