using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MovimentosManual.API;
using MovimentosManual.Domain.Entities;
using Xunit;

namespace MovimentosManual.Tests.Integration
{
    public class MovimentoManualApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MovimentoManualApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CriaMovimentoManual()
        {
            var movimento = new MovimentoManual
            {
                Mes = 7,
                Ano = 2025,
                NumeroLancamento = 99,
                CodigoProduto = "P001",
                CodigoCosif = "COSIF001",
                Descricao = "Teste API",
                DataMovimento = DateTime.Now,
                CodigoUsuario = "admin",
                Valor = 999.99M
            };

            var response = await _client.PostAsJsonAsync("/api/MovimentoManual", movimento);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetById_DeveRetornarMovimento()
        {
            var response = await _client.GetAsync("/api/MovimentoManual/7/2025/99");
            response.EnsureSuccessStatusCode();
            var movimento = await response.Content.ReadFromJsonAsync<MovimentoManual>();
            Assert.Equal(99, movimento?.NumeroLancamento);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOk()
        {
            var response = await _client.GetAsync("/api/MovimentoManual");
            response.EnsureSuccessStatusCode();
        }
    }
}
