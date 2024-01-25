using poc.api.redis.Model;

namespace poc.api.redis.Service.Persistence;

public interface IProdutoService
{
    Task<List<Produto>> Get();
    Task<Produto> Get(int id);
    Task<Produto> Post(Produto entity);
    Task<Produto> Put(Produto entity);
    Task<Produto> Delete(int id);
}
