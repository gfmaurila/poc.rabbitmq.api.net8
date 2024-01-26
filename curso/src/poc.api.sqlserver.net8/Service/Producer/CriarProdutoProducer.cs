using poc.api.sqlserver.Model;
using poc.api.sqlserver.Service.MessageBus;
using System.Text;
using System.Text.Json;

namespace poc.api.sqlserver.Service.Producer;

public class CriarProdutoProducer : ICriarProdutoProducer
{
    private readonly IMessageBusService _messageBusService;
    private readonly ILogger<CriarProdutoProducer> _logger;
    private const string QUEUE_NAME = "CRIAR_PRODUTO";
    public CriarProdutoProducer(IMessageBusService messageBusService, ILogger<CriarProdutoProducer> logger)
    {
        _messageBusService = messageBusService;
        _logger = logger;
    }

    public void Publish(Produto model)
    {
        _logger.LogInformation($"Producer > Publish > Produto > CRIAR_PRODUTO > SQL Server...");
        var modelJson = JsonSerializer.Serialize(model);
        var modelBytes = Encoding.UTF8.GetBytes(modelJson);
        _messageBusService.Publish(QUEUE_NAME, modelBytes);
    }
}
