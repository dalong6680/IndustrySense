using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Infrastructure.Data.Entity;

namespace IndustrySense.Server.Application.Services
{
    public interface IParsingRuleService
    {
        void AddParsingRule(ParsingRule parsingRule);
        ParsingRule? GetParsingRuleById(int id);
        ParsingRule? GetParsingRuleByName(string name);
        ResultSet<ParsingRule> GetParsingRules(int pageIndex);
        string? GetParsingRuleScriptById(int id);
        void RemoveParsingRuleById(int id);
        void UpdateParsingRule(int id, ParsingRule updateParsingRule);
    }
}