using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.dapper.Service.Redis;

public interface IProdutoRedisService
{
    Task PostAsync(Produto model);
    Task PutAsync(Produto model);
}
