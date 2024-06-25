using IndustrySense.Server.Models;

namespace IndustrySense.Server.Services
{
    public interface IElectricDataService
    {
        List<ElectricData> GetElectricData();
    }
}
