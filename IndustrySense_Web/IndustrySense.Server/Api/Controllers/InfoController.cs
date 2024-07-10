using IndustrySense.Server.Common.TcpServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IndustrySense.Server.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : Controller
    {
        private readonly ITcpServer _tcpServer;

        public InfoController(ITcpServer tcpServer)
        {
            _tcpServer = tcpServer;
        }

        [HttpGet("onlineDeviceCount")]
        public IActionResult GetOnlineClientCount()
        {
            return Ok(_tcpServer.ClientCount);
        }
    }
}
