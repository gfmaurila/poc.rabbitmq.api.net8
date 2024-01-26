using Microsoft.OpenApi.Models;
using poc.api.redis.Service.Persistence;

namespace poc.api.redis.EndPoints;
public static class ProdutosEndpoints
{
    public static void RegisterProdutosEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/produto", async (IProdutoService _service, ILogger<Program> logger) =>
        {
            logger.LogInformation("Buscando produtos - Redis...");

            var produto = await _service.Get();
            if (produto is null)
            {
                logger.LogWarning("Nenhum produto encontrado - Redis");
                return Results.NotFound();
            }

            logger.LogInformation("Produtos encontrados - Redis: {ProdutoCount}", produto.Count());
            return TypedResults.Ok(produto);
        })
        .WithName("BuscarProdutos")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Buscar produtos",
            Description = "Buscar produtos",
            Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Minha Loja" } }
        });

        app.MapGet("/api/produto/{id}", async (int id, IProdutoService _service, ILogger<Program> logger) =>
        {
            logger.LogInformation("Buscando produtos - Redis...");

            var produto = await _service.Get(id);
            if (produto is null)
            {
                logger.LogWarning("Nenhum produto encontrado - Redis");
                return Results.NotFound();
            }

            logger.LogInformation("Produtos encontrados - Redis: {produto}", produto);
            return Results.Ok(produto);
        })
        .WithName("BuscarProdutoId")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Buscar produto pelo id",
            Description = "Buscar produto pelo id",
            Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Minha Loja" } }
        });
    }
}
