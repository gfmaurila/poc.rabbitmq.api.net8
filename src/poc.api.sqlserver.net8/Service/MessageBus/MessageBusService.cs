using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

namespace poc.api.sqlserver.Service.MessageBus;
public class MessageBusService : IMessageBusService
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessageBusService> _logger;

    public MessageBusService(IConfiguration configuration, ILogger<MessageBusService> logger)
    {
        _configuration = configuration;
        _connectionFactory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQConnection:Host"],
            UserName = _configuration["RabbitMQConnection:Username"],
            Password = _configuration["RabbitMQConnection:Password"]
        };
        _logger = logger;
    }
    public void Publish(string queue, byte[] message)
    {
        _logger.LogInformation("MessageBusService > Publish > Produto > SQL Server...");
        using (var connection = _connectionFactory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: queue,
                    basicProperties: null,
                    body: message);
            }
        }
    }
}
