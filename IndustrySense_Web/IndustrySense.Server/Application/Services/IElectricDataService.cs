using IndustrySense.Server.Models;

namespace IndustrySense.Server.Application.Services
{
    public interface IElectricDataService
    {
        List<ElectricData> GetElectricData();
    }
}
