using IndustrySense.Server.Application.Services;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("add")]
        public IActionResult AddDevice([FromQuery] Device device)
        {
            _deviceService.AddDevice(device);
            return Ok("Device added successfully");
        }

        [HttpGet("remove/{id}")]
        public IActionResult RemoveDeviceById(int id)
        {
            _deviceService.RemoveDeviceById(id);
            return Ok("Device removed successfully");
        }

        [HttpGet("getbyname")]
        public IActionResult GetDeviceByName([FromQuery] string name)
        {
            var result = _deviceService.GetDeviceByName(name);
            if (result == null)
                return NotFound("Device not found");
            return Ok(result);
        }

        [HttpGet("getbyip")]
        public IActionResult GetDeviceByIpAddress([FromQuery] string ip)
        {
            var result = _deviceService.GetDeviceByIpAddress(ip);
            if (result == null)
                return NotFound("Device not found");
            return Ok(result);
        }

        [HttpGet("getall")]
        public IActionResult GetDevices([FromQuery] int pageIndex)
        {
            var result = _deviceService.GetDevices(pageIndex);
            return Ok(result);
        }

        // public IActionResult UpdateDevice([FromQuery] Expression<Func<Device, bool>> filter, [FromQuery] Action<Device> updateAction)
        // {
        //     _deviceService.UpdateDevice(filter, updateAction);
        //     return Ok("Device updated successfully");
        // }
    }
}
