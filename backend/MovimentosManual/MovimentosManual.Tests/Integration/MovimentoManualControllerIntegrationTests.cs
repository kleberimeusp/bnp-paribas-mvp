using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using MovimentosManual.Application.DTOs;
using MovimentosManual.Tests.TestHost;
using Xunit;

namespace MovimentosManual.Tests.Integration
{
    public class MovimentoManualControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public MovimentoManualControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_ComValorNegativo_DeveRetornar400()
        {
            var request = new MovimentoManualRequest
            {
                Mes = 7,
                Ano = 2025,
                NumeroLancamento = 1,
                CodigoProduto = "P001",
                CodigoCosif = "C001",
                Descricao = "Teste inválido",
                Valor = -100.0m,
                DataCriacao = DateTime.UtcNow
            };

            var response = await _client.PostAsJsonAsync("/api/movimentomanual", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostEGet_DeveInserirEMostrarMovimentoComSucesso()
        {
            var request = new MovimentoManualRequest
            {
                Mes = 7,
                Ano = 2025,
                NumeroLancamento = 101,
                CodigoProduto = "PSEED",       // deve existir no banco in-memory
                CodigoCosif = "CSEED",
                Descricao = "Teste de sucesso",
                Valor = 150.75m,
                DataCriacao = DateTime.UtcNow
            };

            // POST
            var postResponse = await _client.PostAsJsonAsync("/api/movimentomanual", request);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // GET
            var getResponse = await _client.GetAsync("/api/movimentomanual/7/2025/101");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await getResponse.Content.ReadFromJsonAsync<MovimentoManualResponse>();
            result.Should().NotBeNull();
            result!.Descricao.Should().Be("Teste de sucesso");
            result.Valor.Should().Be(150.75m);
        }
    }
}
