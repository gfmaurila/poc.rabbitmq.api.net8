using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using poc.api.sqlserver.Configuration;
using poc.api.sqlserver.dapper.Service.Producer;
using poc.api.sqlserver.Model;
using System.Data;

namespace poc.api.sqlserver.dapper.Service.Persistence;

public class ProdutoService : IProdutoService
{
    private readonly SqlServerDb _db;
    private readonly IProdutoProducer _producer;

    private readonly string _connectionString;

    public ProdutoService(SqlServerDb db, IConfiguration connectionString, IProdutoProducer producer)
    {
        _db = db;
        _connectionString = connectionString.GetConnectionString("SqlConnection");
        _producer = producer;
    }

    public async Task<List<Produto>> Get()
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var produtos = await db.QueryAsync<Produto>(SQL_GET);
        var lista = produtos.ToList();
        return lista;
    }

    public async Task<Produto> Get(int id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var produto = await db.QueryFirstOrDefaultAsync<Produto>(SQL_GET_ID, new { Id = id });
        return produto;
    }

    public async Task<Produto> Post(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        using IDbConnection db = new SqlConnection(_connectionString);

        var id = await db.QuerySingleAsync<int>(SQL_POST, entity);
        entity.Id = id;

        _producer.Publish(entity);

        return entity;
    }

    public async Task<Produto> Put(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        using IDbConnection db = new SqlConnection(_connectionString);

        await db.ExecuteAsync(SQL_PUT, entity);
        return entity;
    }

    public async Task<Produto> Delete(int id)
    {
        var entity = await Get(id);

        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        using IDbConnection db = new SqlConnection(_connectionString);

        await db.ExecuteAsync(SQL_DELETE, new { Id = id });

        return entity;
    }

    #region SQL
    private const string SQL_POST = @"INSERT INTO Produto (Nome, Preco) VALUES (@Nome, @Preco); SELECT CAST(SCOPE_IDENTITY() as int);";
    private const string SQL_PUT = @"UPDATE Produto SET Nome = @Nome, Preco = @Preco WHERE Id = @Id";
    private const string SQL_DELETE = @"DELETE FROM Produto WHERE Id = @Id";
    private const string SQL_GET = @"SELECT * FROM Produto";
    private const string SQL_GET_ID = @"SELECT * FROM Produto WHERE Id = @Id";
    #endregion
}
