using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.Service.Persistence;

public interface IProdutoService
{
    Task<List<Produto>> Get();
    Task<Produto> Get(int id);
    Task<Produto> Post(Produto entity);
    Task<Produto> Put(Produto entity);
    Task<Produto> Delete(int id);
}
