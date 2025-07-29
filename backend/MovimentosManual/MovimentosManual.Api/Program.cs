using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MovimentosManual.Infrastructure.Context;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Application.Services;
using MovimentosManual.Infrastructure.Repositories;
using MovimentosManual.Api.Mappings;
using MovimentosManual.Infrastructure.Helpers;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// CONFIGURAÇÕES
// -----------------------------
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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

// -----------------------------
// SWAGGER COMPLETO
// -----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MovimentosManual API",
        Version = "v1",
        Description = "API para controle de produtos, cosifs e lançamentos manuais."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

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

// HTTPS apenas em produção
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();
app.Run();
