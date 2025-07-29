using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using MovimentosManual.Application.DTOs;
using MovimentosManual.Tests.TestHost;
using Xunit;

namespace MovimentosManual.Tests.Integration
{
    public class CosifControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CosifControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_ComDadosInvalidos_DeveRetornar400()
        {
            var request = new CosifRequest
            {
                CodigoCosif = "",           // inválido
                Descricao = "Teste inválido",
                Status = "X"                // inválido
            };

            var response = await _client.PostAsJsonAsync("/api/cosif", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetPorProduto_DeveRetornarCosifSeedado()
        {
            // GET
            var response = await _client.GetAsync("/api/cosif/produto/PSEED");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var lista = await response.Content.ReadFromJsonAsync<List<ProdutoCosifResponse>>();

            lista.Should().NotBeNull();
            lista!.Should().ContainSingle();
            lista[0].CodigoProduto.Should().Be("PSEED");
            lista[0].CodigoCosif.Should().Be("CSEED");
        }
    }
}
