using Newtonsoft.Json;
using poc.api.redis.Model;
using StackExchange.Redis;

namespace poc.api.redis.Service.Persistence;

public class ProdutoService : IProdutoService
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _multiplexer;

    public ProdutoService(IConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
        _db = multiplexer.GetDatabase();
    }

    public async Task<List<Produto>> Get()
    {
        var server = _multiplexer.GetServer(_multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: "produto:*");

        var produtos = new List<Produto>();

        foreach (var key in keys)
        {
            var value = await _db.StringGetAsync(key);
            if (value.HasValue)
            {
                try
                {
                    var produto = JsonConvert.DeserializeObject<Produto>(value);
                    if (produto != null)
                    {
                        produtos.Add(produto);
                    }
                }
                catch (JsonSerializationException ex)
                {

                }
            }
        }

        return produtos;
    }

    public async Task<Produto> Get(int id)
        => JsonConvert.DeserializeObject<Produto>(await _db.StringGetAsync($"produto:{id}"));

    public async Task<Produto> Post(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var key = $"produto:{entity.Id}";
        var value = JsonConvert.SerializeObject(entity);

        bool setSucess = await _db.StringSetAsync(key, value);

        if (!setSucess)
            throw new Exception("Falha ao salvar produto no redis.");

        return entity;
    }

    public async Task<Produto> Put(Produto entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        if (entity.Id <= 0)
            throw new ArgumentException("ID do produto é inválido");

        var key = $"produto:{entity.Id}";
        var value = JsonConvert.SerializeObject(entity);

        bool setSucess = await _db.StringSetAsync(key, value);

        if (!setSucess)
            throw new Exception("Falha ao atualizar produto no redis.");

        return entity;
    }

    public async Task<Produto> Delete(int id)
    {
        var key = $"produto:{id}";
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
            return null;

        var entity = JsonConvert.DeserializeObject<Produto>(value);
        bool deleteSucess = await _db.KeyDeleteAsync(key);

        if (!deleteSucess)
            throw new Exception("Falha ao deletar produto no redis.");

        return entity;
    }
}
