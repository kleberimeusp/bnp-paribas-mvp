// Pasta: MovimentosManual.Tests/Integration/
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MovimentosManual.API;
using MovimentosManual.Domain.Entities;
using Xunit;

namespace MovimentosManual.Tests.Integration
{
    public class ProdutoApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProdutoApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/produto");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_CreatesProduto()
        {
            var produto = new Produto
            {
                CodigoProduto = "P999",
                Descricao = "Produto Test API",
                Status = "A"
            };

            var response = await _client.PostAsJsonAsync("/api/produto", produto);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetById_ReturnsProduto()
        {
            var response = await _client.GetAsync("/api/produto/P999");
            response.EnsureSuccessStatusCode();
            var produto = await response.Content.ReadFromJsonAsync<Produto>();
            Assert.Equal("P999", produto?.CodigoProduto);
        }
    }
}
