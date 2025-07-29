using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using MovimentosManual.Application.DTOs;
using MovimentosManual.Tests.TestHost;
using Xunit;

namespace MovimentosManual.Tests.Integration
{
    public class ProdutoCosifControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProdutoCosifControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_ComStatusInvalido_DeveRetornar400()
        {
            var request = new ProdutoCosifRequest
            {
                CodigoProduto = "P001",
                CodigoCosif = "C001",
                CodigoClassificacao = "A1",
                Status = "X"  // inválido
            };

            var response = await _client.PostAsJsonAsync("/api/produtocosif", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostEGet_DeveCriarEObterComSucesso()
        {
            var request = new ProdutoCosifRequest
            {
                CodigoProduto = "P100",
                CodigoCosif = "C100",
                CodigoClassificacao = "CL01",
                Status = "A"
            };

            // POST
            var postResponse = await _client.PostAsJsonAsync("/api/produtocosif", request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // GET
            var getResponse = await _client.GetAsync("/api/produtocosif/P100/C100");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await getResponse.Content.ReadFromJsonAsync<ProdutoCosifResponse>();
            response.Should().NotBeNull();
            response!.CodigoProduto.Should().Be("P100");
            response.CodigoCosif.Should().Be("C100");
            response.CodigoClassificacao.Should().Be("CL01");
        }
    }
}
