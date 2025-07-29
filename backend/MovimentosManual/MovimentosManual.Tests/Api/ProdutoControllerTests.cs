using System.Net;
using System.Net.Http.Json;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Tests.Shared;
using Xunit;

namespace MovimentosManual.Tests.Api
{
    public class ProdutoControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProdutoControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Produto_DeveRetornarCreated()
        {
            var produto = new Produto
            {
                CodigoProduto = "T123",
                Descricao = "Teste Produto",
                Status = "A"
            };

            var response = await _client.PostAsJsonAsync("/api/produto", produto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Get_Produto_DeveRetornarLista()
        {
            var response = await _client.GetAsync("/api/produto");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
