using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.Statements.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MassTransitController : ControllerBase
    {
        readonly IPublishEndpoint _publishEndpoint;

        public MassTransitController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string value)
        {
            await _publishEndpoint.Publish<EnteredValue>(new EnteredValue
            {
                Value = value
            });

            return Ok();
        }

        private class EnteredValue
        {
            public string Value { get; set; }
        }
    }
}
