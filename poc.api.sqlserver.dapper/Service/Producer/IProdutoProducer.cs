using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.dapper.Service.Producer;

public interface IProdutoProducer
{
    void Publish(Produto model);
}
