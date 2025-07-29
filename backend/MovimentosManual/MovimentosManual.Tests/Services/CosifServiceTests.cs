using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using MovimentosManual.Application.Services;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Domain.Interfaces;
using Xunit;

namespace MovimentosManual.Tests.Services
{
    public class CosifServiceTests
    {
        private readonly Mock<IRepository<Cosif>> _repositoryMock;
        private readonly CosifService _service;

        public CosifServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Cosif>>();
            _service = new CosifService(_repositoryMock.Object);
        }

        [Fact]
        public async Task ObterPorCodigo_DeveRetornarCosif_SeExistir()
        {
            var cosif = new Cosif { CodigoCosif = "C001", Status = "A" };
            _repositoryMock.Setup(r => r.ListarTodos()).ReturnsAsync(new List<Cosif> { cosif });

            var result = await _service.ObterPorCodigo("C001");

            result.Should().NotBeNull();
            result!.CodigoCosif.Should().Be("C001");
        }

        [Fact]
        public async Task ObterPorCodigo_DeveRetornarNull_SeNaoEncontrar()
        {
            _repositoryMock.Setup(r => r.ListarTodos()).ReturnsAsync(new List<Cosif>());

            var result = await _service.ObterPorCodigo("C999");

            result.Should().BeNull();
        }

        [Fact]
        public async Task Incluir_DeveChamarRepositorio()
        {
            var cosif = new Cosif { CodigoCosif = "C001" };
            await _service.Incluir(cosif);

            _repositoryMock.Verify(r => r.Incluir(cosif), Times.Once);
        }

        [Fact]
        public async Task Atualizar_DeveChamarRepositorio()
        {
            var cosif = new Cosif { CodigoCosif = "C001" };
            await _service.Atualizar(cosif);

            _repositoryMock.Verify(r => r.Atualizar(cosif), Times.Once);
        }

        [Fact]
        public async Task Remover_DeveChamarRemocao_SeEncontrar()
        {
            var cosif = new Cosif { CodigoCosif = "C001" };
            _repositoryMock.Setup(r => r.ListarTodos()).ReturnsAsync(new List<Cosif> { cosif });

            await _service.Remover("C001");

            _repositoryMock.Verify(r => r.Remover(cosif), Times.Once);
        }

        [Fact]
        public async Task Remover_NaoChamaRemocao_SeNaoEncontrar()
        {
            _repositoryMock.Setup(r => r.ListarTodos()).ReturnsAsync(new List<Cosif>());

            await _service.Remover("C001");

            _repositoryMock.Verify(r => r.Remover(It.IsAny<Cosif>()), Times.Never);
        }
    }
}
