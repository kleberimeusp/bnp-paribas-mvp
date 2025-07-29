using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using MovimentosManual.Infrastructure.Context;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Infrastructure.Services;
using System.Threading.Tasks;

namespace MovimentosManual.Tests.Services
{
    public class ProdutoServiceTests
    {
        private readonly ProdutoService _service;
        private readonly MovimentosDbContext _context;

        public ProdutoServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovimentosDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Produto")
                .Options;

            _context = new MovimentosDbContext(options);
            _service = new ProdutoService(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduto()
        {
            var produto = new Produto
            {
                CodigoProduto = "P001",
                Descricao = "Produto Teste",
                Status = "A"
            };

            await _service.AddAsync(produto);

            var result = await _service.GetByIdAsync("P001");
            Assert.NotNull(result);
            Assert.Equal("Produto Teste", result?.Descricao);
        }
    }
}
