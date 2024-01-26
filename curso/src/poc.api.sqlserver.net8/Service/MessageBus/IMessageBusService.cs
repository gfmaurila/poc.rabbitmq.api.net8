namespace poc.api.sqlserver.Service.MessageBus;
public interface IMessageBusService
{
    void Publish(string queue, byte[] message);
}
