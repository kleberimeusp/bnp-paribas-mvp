using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Api.Controllers;
using MovimentosManual.Application.DTOs;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovimentosManual.Tests.Controllers
{
    public class ProdutoControllerTests
    {
        private readonly Mock<IProdutoService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProdutoController _controller;

        public ProdutoControllerTests()
        {
            _mockService = new Mock<IProdutoService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ProdutoController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Get_DeveRetornarTodosOsProdutos()
        {
            // Arrange
            var produtos = new List<Produto> { new Produto { CodigoProduto = "001", Descricao = "Teste" } };
            var responses = new List<ProdutoResponse> { new ProdutoResponse { CodigoProduto = "001", Descricao = "Teste" } };

            _mockService.Setup(s => s.ListarTodos()).ReturnsAsync(produtos);
            _mockMapper.Setup(m => m.Map<List<ProdutoResponse>>(produtos)).Returns(responses);

            // Act
            var result = await _controller.Get();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsAssignableFrom<List<ProdutoResponse>>(ok.Value);
            Assert.Single(data);
        }

        [Fact]
        public async Task Get_CodigoExistente_DeveRetornarProduto()
        {
            var entity = new Produto { CodigoProduto = "001", Descricao = "Teste" };
            var response = new ProdutoResponse { CodigoProduto = "001", Descricao = "Teste" };

            _mockService.Setup(s => s.Obter("001")).ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<ProdutoResponse>(entity)).Returns(response);

            var result = await _controller.Get("001");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var data = Assert.IsType<ProdutoResponse>(ok.Value);
            Assert.Equal("001", data.CodigoProduto);
        }

        [Fact]
        public async Task Get_CodigoInexistente_DeveRetornarNotFound()
        {
            _mockService.Setup(s => s.Obter("999")).ReturnsAsync((Produto?)null);

            var result = await _controller.Get("999");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_DeveCriarProduto()
        {
            var request = new ProdutoRequest { CodigoProduto = "123", Descricao = "Teste" };
            var entity = new Produto { CodigoProduto = "123", Descricao = "Teste" };
            var response = new ProdutoResponse { CodigoProduto = "123", Descricao = "Teste" };

            _mockMapper.Setup(m => m.Map<Produto>(request)).Returns(entity);
            _mockMapper.Setup(m => m.Map<ProdutoResponse>(entity)).Returns(response);

            var result = await _controller.Post(request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var data = Assert.IsType<ProdutoResponse>(created.Value);
            Assert.Equal("123", data.CodigoProduto);
        }

        [Fact]
        public async Task Put_CodigoInvalido_DeveRetornarBadRequest()
        {
            var request = new ProdutoRequest { CodigoProduto = "001", Descricao = "Produto A" };
            var result = await _controller.Put("999", request);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_DeveRetornarNoContent()
        {
            _mockService.Setup(s => s.Remover("001")).Returns(Task.CompletedTask);
            var result = await _controller.Delete("001");
            Assert.IsType<NoContentResult>(result);
        }
    }
}
