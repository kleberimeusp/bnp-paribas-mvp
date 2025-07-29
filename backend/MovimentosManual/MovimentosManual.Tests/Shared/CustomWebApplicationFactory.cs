using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovimentosManual.Infrastructure.Context;

namespace MovimentosManual.Tests.Shared
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MovimentosDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<MovimentosDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        }
    }
}
