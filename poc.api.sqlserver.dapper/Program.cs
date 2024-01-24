using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using poc.api.sqlserver.Configuration;
using poc.api.sqlserver.dapper.Service.Consumers;
using poc.api.sqlserver.dapper.Service.MessageBus;
using poc.api.sqlserver.dapper.Service.Persistence;
using poc.api.sqlserver.dapper.Service.Producer;
using poc.api.sqlserver.dapper.Service.Redis;
using poc.api.sqlserver.EndPoints;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddConnections();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(builder.Configuration);

// Sql Server
builder.Services.AddDbContext<SqlServerDb>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Dapper
builder.Services.AddTransient<IDbConnection>(op => new SqlConnection(builder.Configuration.GetConnectionString("SqlConnection")));

// Service 
builder.Services.AddScoped<IProdutoService, ProdutoService>();

// Bus
builder.Services.AddScoped<IMessageBusService, MessageBusService>();
builder.Services.AddScoped<IProdutoProducer, ProdutoProducer>();
builder.Services.AddHostedService<ProdutoConsumer>();
builder.Services.AddScoped<IProdutoRedisService, ProdutoRedisService>();

var app = builder.Build();

app.UseHttpsRedirection();

// Configura middlewere
app.UseStatusCodePages(async statusCodeContext => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode).ExecuteAsync(statusCodeContext.HttpContext));

app.RegisterProdutosEndpoints();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();