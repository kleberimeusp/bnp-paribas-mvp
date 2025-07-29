using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using MovimentosManual.Application.DTOs;
using MovimentosManual.Tests.TestHost;
using Xunit;

namespace MovimentosManual.Tests.Integration
{
    public class ProdutoControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProdutoControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_ComCodigoProdutoVazio_DeveRetornar400()
        {
            var request = new ProdutoRequest
            {
                CodigoProduto = "", // inválido
                Descricao = "Produto com erro",
                Status = "A"
            };

            var response = await _client.PostAsJsonAsync("/api/produto", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostEGet_DeveCriarEObterProdutoComSucesso()
        {
            var request = new ProdutoRequest
            {
                CodigoProduto = "P999",
                Descricao = "Produto Teste Integração",
                Status = "A"
            };

            // POST
            var postResponse = await _client.PostAsJsonAsync("/api/produto", request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // GET
            var getResponse = await _client.GetAsync("/api/produto/P999");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await getResponse.Content.ReadFromJsonAsync<ProdutoResponse>();
            result.Should().NotBeNull();
            result!.CodigoProduto.Should().Be("P999");
            result.Descricao.Should().Be("Produto Teste Integração");
        }
    }
}
