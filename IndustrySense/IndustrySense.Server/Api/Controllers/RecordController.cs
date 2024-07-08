using IndustrySense.Server.Application.Services;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService _recordService;

        public RecordController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("add")]
        public IActionResult AddRecord([FromQuery] Record record)
        {
            _recordService.AddRecord(record);
            return Ok("Record added successfully");
        }

        [HttpGet("remove/{id}")]
        public IActionResult RemoveRecordById(int id)
        {
            _recordService.RemoveRecordById(id);
            return Ok("Record removed successfully");
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetRecordById(int id)
        {
            var result = _recordService.GetRecordById(id);
            if (result == null)
                return NotFound("Record not found");
            return Ok(result);
        }

        [HttpGet("getall")]
        public IActionResult GetRecords([FromQuery] int pageIndex)
        {
            var result = _recordService.GetRecords(pageIndex);
            return Ok(result);
        }

        [HttpGet("update")]
        public IActionResult UpdateRecord([FromQuery] int id, [FromQuery] Record updatedRecord)
        {
            _recordService.UpdateRecord(id, updatedRecord);
            return Ok("Record updated successfully");
        }
    }
}
