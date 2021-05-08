using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;

namespace MCB.VBO.Microservices.Statements.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatementController : ControllerBase
    {
        private readonly ILogger<StatementController> _logger;

        private readonly IWebHostEnvironment _appEnvironment;

        public StatementController(ILogger<StatementController> logger, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
        }

        [HttpPost("create")]
        public int Create()
        {
            return 0;
        }

        [HttpGet("status")]
        public int GetStatus()
        {
            return 0;
        }

        [HttpGet("download")]
        public IActionResult GetStatementFile()
        {
            HttpClient httpClient = new HttpClient();
            templateServiceClient client = new templateServiceClient("https://localhost:55002/swagger", httpClient);

            var result = client.WeatherForecastAsync();
            result.Wait();

            //return result.Result;
            return new JsonResult("meh");
        }

        [HttpGet("history")]
        public IEnumerable<string> GetHistory()
        {
            return new[] { "1", "2", "3" };
        }
    }
}
