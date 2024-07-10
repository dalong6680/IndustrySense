using IndustrySense.Server.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Api.Controllers
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
