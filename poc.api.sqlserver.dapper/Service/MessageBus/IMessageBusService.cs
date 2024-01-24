namespace poc.api.sqlserver.dapper.Service.MessageBus;

public interface IMessageBusService
{
    void Publish(string queue, byte[] message);
}

