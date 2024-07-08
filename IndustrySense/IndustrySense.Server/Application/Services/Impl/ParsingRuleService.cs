using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Dao;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Application.Services.Impl
{
    public class ParsingRuleService : IParsingRuleService
    {
        private readonly IParsingRuleDao _parsingRuleDao;

        public ParsingRuleService(IParsingRuleDao parsingRuleDao)
        {
            _parsingRuleDao = parsingRuleDao;
        }

        public void AddParsingRule(ParsingRule parsingRule)
        {
            _parsingRuleDao.Insert(parsingRule);
        }

        public void RemoveParsingRuleById(int id)
        {
            _parsingRuleDao.Delete(x => x.ParsingRuleId == id);
        }

        public ParsingRule? GetParsingRuleByName(string name)
        {
            return _parsingRuleDao.Select(x => x.Name.Contains(name));
        }

        public ParsingRule? GetParsingRuleById(int id)
        {
            return _parsingRuleDao.Select(x => x.ParsingRuleId == id);
        }

        public string? GetParsingRuleScriptById(int id)
        {
            return _parsingRuleDao.Select(x => x.ParsingRuleId == id)?.Script;
        }

        public ResultSet<ParsingRule> GetParsingRules(int pageIndex)
        {
            return _parsingRuleDao.SelectFilteredList(x => true, pageIndex);
        }

        public void UpdateParsingRule(int id, ParsingRule updateParsingRule)
        {
            _parsingRuleDao.Update(
                x => x.ParsingRuleId == id,
                parsingRule =>
                {
                    parsingRule.Name = updateParsingRule.Name;
                    parsingRule.Script = updateParsingRule.Script;
                }
            );
        }
    }
}
