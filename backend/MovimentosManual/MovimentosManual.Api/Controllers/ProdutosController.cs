using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Application.Services;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutosController(ProdutoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os produtos cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> Get()
        {
            var produtos = await _service.ListarTodos();
            return Ok(produtos);
        }

        /// <summary>
        /// Obtém um produto pelo código.
        /// </summary>
        /// <param name="codigo">Código do produto</param>
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Produto>> Get(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("Código inválido.");

            var produto = await _service.Obter(codigo);
            return produto is null ? NotFound() : Ok(produto);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.Incluir(produto);
            return CreatedAtAction(nameof(Get), new { codigo = produto.CodigoProduto }, produto);
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        [HttpPut("{codigo}")]
        public async Task<ActionResult> Put(string codigo, [FromBody] Produto produto)
        {
            if (codigo != produto.CodigoProduto)
                return BadRequest("Código do produto não confere com o corpo da requisição.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.Atualizar(produto);
            return NoContent();
        }

        /// <summary>
        /// Remove um produto pelo código.
        /// </summary>
        [HttpDelete("{codigo}")]
        public async Task<ActionResult> Delete(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("Código inválido.");

            await _service.Remover(codigo);
            return NoContent();
        }
    }
}
