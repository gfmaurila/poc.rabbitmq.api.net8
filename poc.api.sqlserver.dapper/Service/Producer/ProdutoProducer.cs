using poc.api.sqlserver.dapper.Service.MessageBus;
using poc.api.sqlserver.Model;
using System.Text;
using System.Text.Json;

namespace poc.api.sqlserver.dapper.Service.Producer;

public class ProdutoProducer : IProdutoProducer
{
    private readonly IMessageBusService _messageBusService;
    private const string QUEUE_NAME = "CRIAR_ALTERAR_PRODUTO";
    public ProdutoProducer(IMessageBusService messageBusService)
    {
        _messageBusService = messageBusService;
    }

    public void Publish(Produto model)
    {
        var modelJson = JsonSerializer.Serialize(model);
        var modelBytes = Encoding.UTF8.GetBytes(modelJson);
        _messageBusService.Publish(QUEUE_NAME, modelBytes);
    }
}
