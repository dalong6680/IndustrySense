using IndustrySense.Server.Application.Dto;
using IndustrySense.Server.Application.Services;
using IndustrySense.Server.Application.Services.Impl;
using IndustrySense.Server.Common.Executor;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordService _recordService;
        private readonly IParsingRuleService _parsingRuleService;
        private readonly IDeviceService _deviceService;

        public RecordController(
            IRecordService recordService,
            IParsingRuleService parsingRuleService,
            IDeviceService deviceService
        )
        {
            _recordService = recordService;
            _parsingRuleService = parsingRuleService;
            _deviceService = deviceService;
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
        public IActionResult GetRecords(int pageIndex)
        {
            var result = _recordService.GetRecords(pageIndex);
            List<RecordVO> lst = new List<RecordVO>();
            foreach (var record in result.ResultList)
            {
                RecordVO recordVO = new RecordVO()
                {
                    Content = record.Content,
                    RecordId = record.RecordId,
                    DeviceId = record.DeviceId,
                    Timestamp = record.Timestamp,
                };
                var device = _deviceService.GetDeviceById(recordVO.DeviceId);
                if (device.ParsingRuleId != 0)
                {
                    string script = _parsingRuleService.GetParsingRuleScriptById(
                        device.ParsingRuleId
                    )!;
                    LuaExecutor executor = new LuaExecutor();
                    var res = executor.ExecuteScript(script, record.Content);
                    var processedRes = res?[0]?.ToString();
                    Console.WriteLine($"Processed message: {processedRes}");
                    recordVO.ParsedContent = processedRes;
                }
                lst.Add(recordVO);
            }
            return Ok(lst);
        }

        [HttpGet("update")]
        public IActionResult UpdateRecord([FromQuery] int id, [FromQuery] Record updatedRecord)
        {
            _recordService.UpdateRecord(id, updatedRecord);
            return Ok("Record updated successfully");
        }
    }
}
