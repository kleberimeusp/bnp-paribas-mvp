using MovimentosManual.Infrastructure.Context;
using MovimentosManual.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using AutoMapper;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ===================== Configurações de Serviços =====================

// Controllers com opções JSON (camelCase + ignora ciclos)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Conexão com SQL Server com retry automático
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MovimentosDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(6)));

// Injeção de dependência dos serviços
builder.Services.AddScoped<MovimentoService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<ProdutoCosifService>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// CORS liberado para frontend (Angular, etc.)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovimentosManual API", Version = "v1" });
});

var app = builder.Build();

// ===================== Pipeline da Aplicação =====================

// Detecta se está rodando em container Docker
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (isDocker)
{
    app.Urls.Add("http://+:80");
}

// Aplica migrations com tratamento de exceção
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<MovimentosDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Erro ao aplicar migrations: {ex.Message}");
        // Swagger continuará funcionando mesmo que o banco esteja indisponível
    }
}

// Middleware de CORS
app.UseCors("AllowFrontend");

// Swagger na raiz do site (http://localhost:5000)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovimentosManual API v1");
    c.RoutePrefix = string.Empty; // Mostra o Swagger na raiz
});


// Authorization (pode ser comentado se não estiver usando autenticação)
app.UseAuthorization();

// Mapeamento de endpoints
app.MapControllers();

app.Run();
