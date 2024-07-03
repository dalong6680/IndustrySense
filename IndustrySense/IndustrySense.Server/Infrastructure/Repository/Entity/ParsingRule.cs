namespace IndustrySense.Server.Infrastructure.Repository.Entity
{
    public class ParsingRule
    {
        public int ParsingRuleId { get; set; }
        public required string Name { get; set; }
        public required string Script { get; set; }
    }
}
