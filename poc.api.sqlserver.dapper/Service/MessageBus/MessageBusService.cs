using RabbitMQ.Client;

namespace poc.api.sqlserver.dapper.Service.MessageBus;

public class MessageBusService : IMessageBusService
{
    private readonly ConnectionFactory _connectionFactory;

    public MessageBusService()
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
        };
    }
    public void Publish(string queue, byte[] message)
    {
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
