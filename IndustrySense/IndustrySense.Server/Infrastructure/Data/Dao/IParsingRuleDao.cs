using System.Linq.Expressions;
using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Infrastructure.Data.Dao
{
    public interface IParsingRuleDao
    {
        void Delete(Expression<Func<ParsingRule, bool>> filter);
        void Insert(ParsingRule entity);
        ParsingRule? Select(Expression<Func<ParsingRule, bool>> filter);
        ResultSet<ParsingRule> SelectFilteredList(
            Expression<Func<ParsingRule, bool>> filter,
            int pageIndex
        );
        void Update(Expression<Func<ParsingRule, bool>> filter, Action<ParsingRule> predicate);
    }
}
