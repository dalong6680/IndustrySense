using System.Text.Json;
using IndustrySense.Server.Application.Dto;

namespace IndustrySense.Server.Common.Parsers
{
    public class SensorParser
    {
        public SensorParser() { }

        public static void Parse(string src)
        {
            try
            {
                SensorDescriptor descriptor = JsonSerializer.Deserialize<SensorDescriptor>(src)!;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return;
            }


            



        }
    }
}
