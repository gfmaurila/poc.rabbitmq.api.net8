using poc.api.redis.Model;
using poc.api.redis.Service.Persistence;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace poc.api.redis.Service.Consumers;

public class CriarProdutoConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string QUEUE_NAME = "CRIAR_PRODUTO";

    public CriarProdutoConsumer(IServiceProvider servicesProvider)
    {
        _serviceProvider = servicesProvider;

        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, eventArgs) =>
        {
            var modelBytes = eventArgs.Body.ToArray();
            var modelJson = Encoding.UTF8.GetString(modelBytes);
            var model = JsonSerializer.Deserialize<Produto>(modelJson);

            await PostAsync(model);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };
        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    public async Task PostAsync(Produto model)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var sendGridService = scope.ServiceProvider.GetRequiredService<IProdutoService>();
            await sendGridService.Post(model);
        }
    }
}
