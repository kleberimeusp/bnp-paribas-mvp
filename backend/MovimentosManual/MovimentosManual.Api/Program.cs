// =================== Program.cs ajustado ===================
using MovimentosManual.Infrastructure.Context;
using MovimentosManual.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar Controllers e configurações JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Verificar se está rodando no Docker
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (isDocker)
{
    builder.WebHost.UseUrls("http://+:80");
}

// String de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MovimentosDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(6)));

// Adicionar serviços
builder.Services.AddScoped<MovimentoService>();
builder.Services.AddScoped<ProdutoService>();          // ✅ Adicionado
builder.Services.AddScoped<ProdutoCosifService>();     // ✅ Adicionado

// Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovimentosDbContext>();
    context.Database.Migrate();
}

// Usar CORS antes de Authorization
app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
