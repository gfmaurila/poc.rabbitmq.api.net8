﻿using poc.api.redis.Model;
using poc.api.redis.Service.Persistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace poc.api.redis.Service.Consumers;
public class RemoverProdutoConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private const string QUEUE_NAME = "REMOVER_PRODUTO";
    private readonly ILogger<RemoverProdutoConsumer> _logger;
    public RemoverProdutoConsumer(IConfiguration configuration, IServiceProvider serviceProvider, ILogger<RemoverProdutoConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;

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
            arguments: null
        );
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Consumer > ExecuteAsync > Produto > REMOVER_PRODUTO > Redis...");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) => 
        {
            var modelBytes = args.Body.ToArray();
            var modelJson = Encoding.UTF8.GetString(modelBytes);
            var model = JsonSerializer.Deserialize<Produto>(modelJson);

            await DeleteAsync(model);
            _channel.BasicAck(args.DeliveryTag, false);
        };

        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    private async Task DeleteAsync(Produto model)
    {
        _logger.LogInformation($"Consumer > ExecuteAsync > DeleteAsync > Produto > REMOVER_PRODUTO > Redis...");
        using var scope = _serviceProvider.CreateScope();
        var scopeService = scope.ServiceProvider.GetRequiredService<IProdutoService>();
        await scopeService.Delete(model.Id);
    }
}
