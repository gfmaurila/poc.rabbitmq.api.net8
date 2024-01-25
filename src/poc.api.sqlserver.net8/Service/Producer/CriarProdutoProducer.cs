﻿using poc.api.sqlserver.Model;
using poc.api.sqlserver.Service.MessageBus;
using System.Text;
using System.Text.Json;

namespace poc.api.sqlserver.Service.Producer;

public class CriarProdutoProducer : ICriarProdutoProducer
{
    private readonly IMessageBusService _messageBusService;
    private const string QUEUE_NAME = "CRIAR_PRODUTO";
    public CriarProdutoProducer(IMessageBusService messageBusService)
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