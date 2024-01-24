using poc.api.sqlserver.Model;
using Polly;
using System.Net;

namespace poc.api.sqlserver.dapper.Service.Redis;

public class ProdutoRedisService : IProdutoRedisService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProdutoRedisService> _logger;
    private readonly IConfiguration _configuration;
    private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy;

    public ProdutoRedisService(HttpClient httpClient, ILogger<ProdutoRedisService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;

        // Configuração da política de tentativas de retry
        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, retryCount, context) =>
                {
                    // Lógica a ser executada a cada tentativa de retry
                    _logger.LogWarning($"Tentativa {retryCount} de envio...");
                }
            );
    }

    public async Task EnviarProdutoRedisAsync(Produto model)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.PostAsJsonAsync(
                _configuration["ProdutosRedis:URL"],
                new Produto
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Preco = model.Preco
                }
            );
            response.EnsureSuccessStatusCode();

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Falha ao enviar produto: {error}");
            }

            return response;
        });
    }
}
