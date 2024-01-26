﻿using Microsoft.EntityFrameworkCore;
using poc.api.sqlserver.Configuration;
using poc.api.sqlserver.Model;
using poc.api.sqlserver.Service.Producer;

namespace poc.api.sqlserver.Service.Persistence;

public class ProdutoService : IProdutoService
{
    private readonly SqlServerDb _db;
    private readonly ICriarProdutoProducer _criarProdutoProducer;
    private readonly IAlterarProdutoProducer _alterarProdutoProducer;
    private readonly IRemoverProdutoProducer _removerProdutoProducer;

    public ProdutoService(SqlServerDb db, ICriarProdutoProducer criarProdutoProducer, IAlterarProdutoProducer alterarProdutoProducer, IRemoverProdutoProducer removerProdutoProducer)
    {
        _db = db;
        _criarProdutoProducer = criarProdutoProducer;
        _alterarProdutoProducer = alterarProdutoProducer;
        _removerProdutoProducer = removerProdutoProducer;
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

        _criarProdutoProducer.Publish(entity);

        return entity;
    }

    public async Task<Produto> Put(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.Produto.Update(entity);
        await _db.SaveChangesAsync();

        _alterarProdutoProducer.Publish(entity);

        return entity;
    }

    public async Task<Produto> Delete(int id)
    {
        var entity = await Get(id);

        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        _db.Produto.Remove(entity);
        await _db.SaveChangesAsync();

        _removerProdutoProducer.Publish(entity);

        return entity;
    }
}