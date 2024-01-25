using Microsoft.EntityFrameworkCore;
using poc.api.sqlserver.Configuration;
using poc.api.sqlserver.Model;
using poc.api.sqlserver.Service.Producer;

namespace poc.api.sqlserver.Service.Persistence;

public class ProdutoService : IProdutoService
{
    private readonly SqlServerDb _db;
    private readonly ICriarProdutoProducer _criarProducer;
    private readonly IAlterarProdutoProducer _alterarProducer;
    private readonly IRemoverProdutoProducer _removerProducer;

    public ProdutoService(SqlServerDb db, ICriarProdutoProducer criarProducer, IAlterarProdutoProducer alterarProducer, IRemoverProdutoProducer removerProducer)
    {
        _db = db;
        _criarProducer = criarProducer;
        _alterarProducer = alterarProducer;
        _removerProducer = removerProducer;
    }

    public async Task<List<Produto>> Get()
        => await _db.Produto.AsNoTracking().ToListAsync();

    public async Task<Produto> Get(int id)
        => await _db.Produto.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Produto> Post(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _db.Produto.AddAsync(entity);
        await _db.SaveChangesAsync();

        _criarProducer.Publish(entity);

        return entity;
    }

    public async Task<Produto> Put(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.Produto.Update(entity);
        await _db.SaveChangesAsync();

        _alterarProducer.Publish(entity);

        return entity;
    }

    public async Task<Produto> Delete(int id)
    {
        var entity = await Get(id);

        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.Produto.Remove(entity);
        await _db.SaveChangesAsync();

        _removerProducer.Publish(entity);

        return entity;
    }
}
