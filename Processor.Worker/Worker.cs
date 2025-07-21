using Azure.Messaging.ServiceBus;
using Shared;
using System.Text.Json;

namespace Processor.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private ServiceBusProcessor? _processor;

        private const string QueueName = "products";

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            string config = _config.GetConnectionString("ServiceBus");

            if (string.IsNullOrWhiteSpace(config) || config == "<FROM_ENV>")
            {
                config = Environment.GetEnvironmentVariable("ConnectionStrings__ServiceBus");
            }

            if (string.IsNullOrWhiteSpace(config))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión para ServiceBus.");
            }

            var client = new ServiceBusClient(config);

            _processor = client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());

            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync(cancellationToken);
            _logger.LogInformation("Worker started and listening to queue.");
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            var message = JsonSerializer.Deserialize<ProductMessage>(body);

            _logger.LogInformation("Mensaje recibido:");
            _logger.LogInformation($"Producto: {message?.Name}, Precio: {message?.Price}");

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error al procesar mensaje");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processor != null)
            {
                await _processor.StopProcessingAsync(cancellationToken);
                await _processor.DisposeAsync();
            }

            _logger.LogInformation("Worker detenido.");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
