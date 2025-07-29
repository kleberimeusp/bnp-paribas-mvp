using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MovimentosManual.Infrastructure.Context;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Application.Services;
using MovimentosManual.Infrastructure.Repositories;
using MovimentosManual.Api.Mappings;
using MovimentosManual.Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// CONFIGURAÇÕES
// -----------------------------
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Strings de conexão
var localConn = builder.Configuration.GetConnectionString("LocalhostConnection");
var dockerConn = builder.Configuration.GetConnectionString("DockerConnection");
string? finalConnectionString = null;

if (SqlConnectionTester.Test(localConn))
{
    finalConnectionString = localConn;
    Console.WriteLine("✅ Usando conexão LOCALHOST");
}
else if (SqlConnectionTester.Test(dockerConn))
{
    finalConnectionString = dockerConn;
    Console.WriteLine("✅ Usando conexão SQLSERVER DOCKER");
}
else
{
    Console.WriteLine("❌ Falha ao conectar ao banco de dados.");
    throw new Exception("Conexão SQL falhou.");
}

// -----------------------------
// SERVIÇOS E DEPENDÊNCIAS
// -----------------------------
builder.Services.AddDbContext<MovimentosDbContext>(options =>
    options.UseSqlServer(finalConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(5);
        sqlOptions.CommandTimeout(60);
    }));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IProdutoCosifService, ProdutoCosifService>();
builder.Services.AddScoped<ICosifService, CosifService>();
builder.Services.AddScoped<IMovimentoManualService, MovimentoManualService>();

builder.Services.AddAutoMapper(typeof(MovimentoManualProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------
// CORS
// -----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// -----------------------------
// CONSTRUÇÃO DO APP
// -----------------------------
var app = builder.Build();

// Middleware de erro SQL amigável
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (SqlException ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("❌ Erro ao acessar o banco de dados.");
        Console.WriteLine($"[SQL ERROR] {ex.Message}");
    }
});

// Swagger e DevTools
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MovimentosManual API v1");
        options.RoutePrefix = string.Empty;
    });
}

// ⚠️ HTTPS somente em produção
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Ativa CORS antes de Authorization
app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();
app.Run();
