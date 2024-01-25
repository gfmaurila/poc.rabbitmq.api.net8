using poc.api.redis.Model;
using poc.api.redis.Service.Persistence;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace poc.api.redis.Service.Consumers;

public class AlterarProdutoConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string QUEUE_NAME = "ALTERAR_PRODUTO";
    private readonly IConfiguration _configuration;
    private readonly ILogger<AlterarProdutoConsumer> _logger;

    public AlterarProdutoConsumer(IServiceProvider servicesProvider, IConfiguration configuration, ILogger<AlterarProdutoConsumer> logger)
    {
        _serviceProvider = servicesProvider;
        _configuration = configuration;

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQConnection:Host"],
            UserName = _configuration["RabbitMQConnection:Username"],
            Password = _configuration["RabbitMQConnection:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consumer > ExecuteAsync > Produto > ALTERAR_PRODUTO > ExecuteAsync - Redis...");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, eventArgs) =>
        {
            var modelBytes = eventArgs.Body.ToArray();
            var modelJson = Encoding.UTF8.GetString(modelBytes);
            var model = JsonSerializer.Deserialize<Produto>(modelJson);

            await PutAsync(model);
            _logger.LogInformation($"Consumer > ExecuteAsync > Produto > ALTERAR_PRODUTO > ExecuteAsync - Redis... {model}");

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };
        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    public async Task PutAsync(Produto model)
    {
        _logger.LogInformation("Consumer > ExecuteAsync > Produto > ALTERAR_PRODUTO > PutAsync - Redis...");
        using (var scope = _serviceProvider.CreateScope())
        {
            var sendGridService = scope.ServiceProvider.GetRequiredService<IProdutoService>();
            await sendGridService.Put(model);
            _logger.LogInformation($"Consumer > ExecuteAsync > Produto > ALTERAR_PRODUTO > PutAsync - Redis... {model}");
        }
    }
}
