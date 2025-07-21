using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Text.Json;

namespace Sender.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SenderController : ControllerBase
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private const string QueueName = "products";

        public SenderController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ServiceBus");
            _client = new ServiceBusClient(connectionString);
            _sender = _client.CreateSender(QueueName);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ProductMessage message)
        {
            var jsonBody = JsonSerializer.Serialize(message);
            var busMessage = new ServiceBusMessage(jsonBody);
            await _sender.SendMessageAsync(busMessage);
            return Ok("Mensaje enviado a la cola.");
        }
    }
}
