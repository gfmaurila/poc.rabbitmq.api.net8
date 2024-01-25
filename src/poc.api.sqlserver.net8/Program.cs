using Microsoft.EntityFrameworkCore;
using poc.api.sqlserver.Configuration;
using poc.api.sqlserver.EndPoints;
using poc.api.sqlserver.Service.MessageBus;
using poc.api.sqlserver.Service.Persistence;
using poc.api.sqlserver.Service.Producer;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddConnections();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(builder.Configuration);

// Sql Server
builder.Services.AddDbContext<SqlServerDb>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Service 
builder.Services.AddScoped<IProdutoService, ProdutoService>();

// Bus
builder.Services.AddScoped<IMessageBusService, MessageBusService>();
builder.Services.AddScoped<ICriarProdutoProducer, CriarProdutoProducer>();
builder.Services.AddScoped<IAlterarProdutoProducer, AlterarProdutoProducer>();
builder.Services.AddScoped<IRemoverProdutoProducer, RemoverProdutoProducer>();

var app = builder.Build();

app.UseHttpsRedirection();

// Configura middlewere
app.UseStatusCodePages(async statusCodeContext => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode).ExecuteAsync(statusCodeContext.HttpContext));

// EndPoints
app.RegisterProdutosEndpoints();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();