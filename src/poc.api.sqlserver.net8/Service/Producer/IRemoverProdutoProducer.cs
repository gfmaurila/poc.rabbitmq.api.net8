using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.Service.Producer;

public interface IRemoverProdutoProducer
{
    void Publish(Produto model);
}