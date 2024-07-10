namespace IndustrySense.Server.Infrastructure.Data.Entity
{
    public class ParsingRule
    {
        public int ParsingRuleId { get; set; }
        public required string Name { get; set; }
        public required string Script { get; set; }
    }
}
