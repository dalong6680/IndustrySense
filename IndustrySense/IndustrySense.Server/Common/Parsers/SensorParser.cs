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
                SensorData descriptor = JsonSerializer.Deserialize<SensorData>(src)!;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return;
            }


            



        }
    }
}
