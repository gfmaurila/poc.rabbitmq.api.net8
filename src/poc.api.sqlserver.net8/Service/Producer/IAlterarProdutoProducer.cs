using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.Service.Producer;

public interface IAlterarProdutoProducer
{
    void Publish(Produto model);
}