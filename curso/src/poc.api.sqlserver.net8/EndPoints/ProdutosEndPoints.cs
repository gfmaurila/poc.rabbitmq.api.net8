using Microsoft.OpenApi.Models;
using poc.api.sqlserver.Model;
using poc.api.sqlserver.Service.Persistence;

namespace poc.api.sqlserver.EndPoints;
public static class ProdutosEndpoints
{
    public static void RegisterProdutosEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/produto", async (IProdutoService _service, ILogger<Program> logger) =>
        {
            logger.LogInformation("Buscando produtos - SQL Server...");

            var produto = await _service.Get();
            if (produto is null)
            {
                logger.LogWarning("Nenhum produto encontrado - SQL Server");
                return Results.NotFound();
            }

            logger.LogInformation("Produtos encontrados - SQL Server: {ProdutoCount}", produto.Count());
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
            logger.LogInformation("Buscando produtos - SQL Server...");

            var produto = await _service.Get(id);
            if (produto is null)
            {
                logger.LogWarning("Nenhum produto encontrado - SQL Server");
                return Results.NotFound();
            }

            logger.LogInformation("Produtos encontrados - SQL Server: {produto}", produto);
            return Results.Ok(produto);
        })
        .WithName("BuscarProdutoId")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Buscar produto pelo id",
            Description = "Buscar produto pelo id",
            Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Minha Loja" } }
        });

        app.MapPost("/api/produto", async (Produto entity, IProdutoService _service, ILogger<Program> logger) =>
        {
            logger.LogInformation("Cadastro de produtos - SQL Server...");
            if (entity is null)
            {
                logger.LogWarning("Objeto Vazio - SQL Server");
                return Results.NotFound();
            }
            logger.LogInformation("Produto Cadastrado - SQL Server: {entity}", entity);
            return Results.Created($"{entity.Id}", await _service.Post(entity));
        })
        .WithName("CadastrarProduto")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Cadastrar produto",
            Description = "Cadastrar produto",
            Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Minha Loja" } }
        });

        app.MapPut("/api/produto", async (Produto entity, IProdutoService _service, ILogger<Program> logger) =>
        {
            logger.LogInformation("Alterar produtos - SQL Server...");
            if (entity is null)
            {
                logger.LogWarning("Objeto Vazio - SQL Server");
                return Results.NotFound();
            }
            logger.LogInformation("Produto alterado - SQL Server: {entity}", entity);
            return Results.Ok(await _service.Put(entity));
        })
        .WithName("EditarProduto")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Editar produto",
            Description = "Editar produto",
            Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Minha Loja" } }
        });

        app.MapDelete("/api/produto/{id}", async (int id, IProdutoService _service, ILogger<Program> logger) =>
        {
            var produto = await _service.Delete(id);
            logger.LogInformation($"Produto id={id} deletado - SQL Server");
            return Results.Ok($"Produto id={id} deletado");
        })
        .WithName("DeletarProduto")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Deletar produto",
            Description = "Deletar produto",
            Tags = new List<OpenApiTag> { new OpenApiTag { Name = "Minha Loja" } }
        });
    }
}
