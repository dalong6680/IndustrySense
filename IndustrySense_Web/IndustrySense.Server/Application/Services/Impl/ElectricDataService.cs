using IndustrySense.Server.Application.Services;
using IndustrySense.Server.Models;

namespace IndustrySense.Server.Application.Services.Impl
{
    public class ElectricDataService : IElectricDataService
    {
        public List<ElectricData> GetElectricData()
        {
            return new List<ElectricData>
            {
                new ElectricData
                {
                    Index = 1,
                    Name = "电压",
                    Value = "220V"
                },
                new ElectricData
                {
                    Index = 2,
                    Name = "电流",
                    Value = "5A"
                },
                new ElectricData
                {
                    Index = 3,
                    Name = "功率",
                    Value = "1100W"
                }
            };
        }
    }
}
