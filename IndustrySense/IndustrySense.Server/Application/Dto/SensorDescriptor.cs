namespace IndustrySense.Server.Application.Dto
{
    public class SensorDescriptor
    {
        public required string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public required string Content { get; set; }
    }
}
