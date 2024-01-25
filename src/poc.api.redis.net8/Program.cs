using poc.api.redis.Configuration;
using poc.api.redis.EndPoints;
using poc.api.redis.Service.Consumers;
using poc.api.redis.Service.Persistence;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddConnections();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(builder.Configuration);


// Redis
string redisConfiguration = builder.Configuration.GetSection("Redis:Configuration").Value;
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

// Bus
builder.Services.AddHostedService<CriarProdutoConsumer>();
builder.Services.AddHostedService<AlterarProdutoConsumer>();
builder.Services.AddHostedService<RemoverProdutoConsumer>();

// Repository
builder.Services.AddScoped<IProdutoService, ProdutoService>();

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.RegisterProdutosEndpoints();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();