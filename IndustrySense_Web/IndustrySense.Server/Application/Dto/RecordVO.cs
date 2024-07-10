namespace IndustrySense.Server.Application.Dto
{
    public class RecordVO
    {
        public int RecordId { get; set; }
        public DateTime Timestamp { get; set; }
        public required string Content { get; set; }
        public int DeviceId { get; set; }
        public string ParsedContent { get; set; }
    }
}
