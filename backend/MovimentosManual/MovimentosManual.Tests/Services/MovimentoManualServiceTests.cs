using Xunit;
using Microsoft.EntityFrameworkCore;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Infrastructure.Context;
using MovimentosManual.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace MovimentosManual.Tests.Services
{
    public class MovimentoManualServiceTests
    {
        private readonly MovimentoManualService _service;

        public MovimentoManualServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovimentosDbContext>()
                .UseInMemoryDatabase("TestDb_Movimento")
                .Options;

            var context = new MovimentosDbContext(options);

            context.Produtos.Add(new Produto { CodigoProduto = "P001", Descricao = "Produto", Status = "A" });
            context.Cosifs.Add(new Cosif { CodigoCosif = "COS001", Descricao = "Cosif", Status = "A" });
            context.SaveChanges();

            _service = new MovimentoManualService(context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddMovimento()
        {
            var movimento = new MovimentoManual
            {
                Mes = 7,
                Ano = 2025,
                NumeroLancamento = 1,
                CodigoProduto = "P001",
                CodigoCosif = "COS001",
                Descricao = "Teste",
                DataMovimento = DateTime.Now,
                CodigoUsuario = "admin",
                Valor = 99.99M
            };

            await _service.AddAsync(movimento);

            var result = await _service.GetByIdAsync(7, 2025, 1);
            Assert.NotNull(result);
            Assert.Equal("Teste", result?.Descricao);
        }
    }
}
