using IndustrySense.Server.Application.Services;
using IndustrySense.Server.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParsingRuleController : ControllerBase
    {
        private readonly IParsingRuleService _parsingRuleService;

        public ParsingRuleController(IParsingRuleService parsingRuleService)
        {
            _parsingRuleService = parsingRuleService;
        }

        [HttpGet("add")]
        public IActionResult AddParsingRule([FromQuery] ParsingRule parsingRule)
        {
            _parsingRuleService.AddParsingRule(parsingRule);
            return Ok("规则添加成功");
        }

        [HttpGet("remove/{id}")]
        public IActionResult RemoveParsingRuleById(int id)
        {
            _parsingRuleService.RemoveParsingRuleById(id);
            return Ok("ParsingRule removed successfully");
        }

        [HttpGet("getbyname")]
        public IActionResult GetParsingRuleByName([FromQuery] string name)
        {
            var result = _parsingRuleService.GetParsingRuleByName(name);
            if (result == null)
                return NotFound("ParsingRule not found");
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetParsingRuleById(int id)
        {
            var result = _parsingRuleService.GetParsingRuleById(id);
            if (result == null)
                return NotFound("ParsingRule not found");
            return Ok(result);
        }

        [HttpGet("getscriptbyid/{id}")]
        public IActionResult GetParsingRuleScriptById(int id)
        {
            var result = _parsingRuleService.GetParsingRuleScriptById(id);
            if (result == null)
                return NotFound("Script not found");
            return Ok(result);
        }

        [HttpGet("getall")]
        public IActionResult GetParsingRules([FromQuery] int pageIndex)
        {
            var result = _parsingRuleService.GetParsingRules(pageIndex);
            return Ok(result);
        }

        [HttpGet("update")]
        public IActionResult UpdateParsingRule(
            [FromQuery] int id,
            [FromQuery] ParsingRule updateParsingRule
        )
        {
            _parsingRuleService.UpdateParsingRule(id, updateParsingRule);
            return Ok("ParsingRule updated successfully");
        }
    }
}
