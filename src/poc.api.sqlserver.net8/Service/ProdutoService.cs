using Microsoft.EntityFrameworkCore;
using poc.api.sqlserver.Configuration;
using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.Service;

public class ProdutoService : IProdutoService
{
    private readonly SqlServerDb _db;

    public ProdutoService(SqlServerDb db)
    {
        _db = db;
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

        return entity;
    }

    public async Task<Produto> Put(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.Produto.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task<Produto> Delete(int id)
    {
        var entity = await Get(id);

        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.Produto.Remove(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}
