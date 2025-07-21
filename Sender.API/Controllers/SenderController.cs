using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using System.Text.Json;

namespace Sender.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SenderController : ControllerBase
    {
        private readonly ServiceBusSender _sender;
        private readonly ILogger<SenderController> _logger;
        private readonly IConfiguration _config;
        public SenderController(IConfiguration config, ILogger<SenderController> logger)
        {
            _logger = logger;
            _config = config;

            var connectionString = _config.GetConnectionString("ServiceBus");

            if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "<FROM_ENV>")
            {
                connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ServiceBus");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión para ServiceBus.");
            }

            try
            {
                var client = new ServiceBusClient(connectionString);
                _sender = client.CreateSender("products");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el ServiceBusClient o ServiceBusSender.");
                throw;
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ProductMessage message)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(message);
                var busMessage = new ServiceBusMessage(jsonBody);

                await _sender.SendMessageAsync(busMessage);

                _logger.LogInformation("Mensaje enviado correctamente a la cola.");
                return Ok("Mensaje enviado a la cola.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando el mensaje al Service Bus.");
                return StatusCode(500, "Error interno al enviar el mensaje.");
            }
        }
    }
}
