using poc.api.sqlserver.Model;
using poc.api.sqlserver.Service.MessageBus;
using System.Text;
using System.Text.Json;

namespace poc.api.sqlserver.Service.Producer;

public class RemoverProdutoProducer : IRemoverProdutoProducer
{
    private readonly IMessageBusService _messageBusService;
    private readonly ILogger<RemoverProdutoProducer> _logger;
    private const string QUEUE_NAME = "REMOVER_PRODUTO";
    public RemoverProdutoProducer(IMessageBusService messageBusService, ILogger<RemoverProdutoProducer> logger)
    {
        _messageBusService = messageBusService;
        _logger = logger;
    }

    public void Publish(Produto model)
    {
        _logger.LogInformation($"Producer > Publish > Produto > REMOVER_PRODUTO > ExecuteAsync - SQL Server... {model}");
        var modelJson = JsonSerializer.Serialize(model);
        var modelBytes = Encoding.UTF8.GetBytes(modelJson);
        _messageBusService.Publish(QUEUE_NAME, modelBytes);
    }
}