namespace IndustrySense.Server.Infrastructure.Data.Entity
{
    public class Record
    {
        public int RecordId { get; set; }
        public DateTime Timestamp { get; set; }
        public required string Content { get; set; }
        public int ParsingRuleId { get; set; }

        // 外键
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
