using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.Service.Producer;

public interface ICriarProdutoProducer
{
    void Publish(Produto model);
}