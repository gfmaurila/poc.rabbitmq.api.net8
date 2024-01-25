using poc.api.sqlserver.Model;
using poc.api.sqlserver.Service.MessageBus;
using System.Text;
using System.Text.Json;

namespace poc.api.sqlserver.Service.Producer;

public class AlterarProdutoProducer : IAlterarProdutoProducer
{
    private readonly IMessageBusService _messageBusService;
    private readonly ILogger<AlterarProdutoProducer> _logger;
    private const string QUEUE_NAME = "ALTERAR_PRODUTO";
    public AlterarProdutoProducer(IMessageBusService messageBusService, ILogger<AlterarProdutoProducer> logger)
    {
        _messageBusService = messageBusService;
        _logger = logger;
    }

    public void Publish(Produto model)
    {
        _logger.LogInformation($"Producer > Publish > Produto > ALTERAR_PRODUTO > ExecuteAsync - SQL Server... {model}");
        var modelJson = JsonSerializer.Serialize(model);
        var modelBytes = Encoding.UTF8.GetBytes(modelJson);
        _messageBusService.Publish(QUEUE_NAME, modelBytes);
    }
}