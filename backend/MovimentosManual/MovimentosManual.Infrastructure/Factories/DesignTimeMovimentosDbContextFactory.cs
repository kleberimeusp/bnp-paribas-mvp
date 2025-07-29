using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using MovimentosManual.Infrastructure.Context;

namespace MovimentosManual.Infrastructure.Factories
{
    public class DesignTimeMovimentosDbContextFactory : IDesignTimeDbContextFactory<MovimentosDbContext>
    {
        public MovimentosDbContext CreateDbContext(string[] args)
        {
            // Caminho da raiz do projeto API onde está o appsettings.json
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "MovimentosManual.Api"));

            // Constrói a configuração carregando o appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Recupera a connection string do appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<MovimentosDbContext>();

            optionsBuilder.UseSqlServer(connectionString, opts =>
            {
                opts.EnableRetryOnFailure(); // recomendação de resiliência
            });

            return new MovimentosDbContext(optionsBuilder.Options);
        }
    }
}
