using IndustrySense.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectricDataController(IElectricDataService electricDataService) : ControllerBase
    {
        private readonly IElectricDataService _electricDataService = electricDataService;

        [HttpGet]
        public IActionResult GetElectricData()
        {
            var electricData = _electricDataService.GetElectricData();
            return Ok(electricData);
        }
    }
}
