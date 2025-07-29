using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Core.Pagination;
using AutoMapper;
using MovimentosManual.Infrastructure.Common.Linq;
using MovimentosManual.Core.Common;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Application.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _service;
        private readonly IMapper _mapper;

        public ProdutoController(IProdutoService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos os produtos cadastrados.
        /// </summary>
        /// <returns>Lista de produtos.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProdutoResponse>), 200)]
        public async Task<ActionResult<List<ProdutoResponse>>> GetAll()
        {
            var produtos = await _service.ListarTodos();
            return Ok(_mapper.Map<List<ProdutoResponse>>(produtos));
        }

        /// <summary>
        /// Retorna um produto pelo código.
        /// </summary>
        /// <param name="codigo">Código do produto</param>
        /// <returns>Produto encontrado ou 404.</returns>
        [HttpGet("{codigo}")]
        [ProducesResponseType(typeof(ProdutoResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProdutoResponse>> GetByCodigo(string codigo)
        {
            var produto = await _service.Obter(codigo);
            return produto is null ? NotFound() : Ok(_mapper.Map<ProdutoResponse>(produto));
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <param name="request">Dados do produto</param>
        /// <returns>Produto criado ou erro de validação/duplicação.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Produto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] ProdutoRequest request)
        {
            if (request == null)
                return BadRequest("Requisição inválida.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigo = request.CodigoProduto?.Trim();
            var descricao = request.Descricao?.Trim();
            var status = request.Status?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("Código do produto é obrigatório.");

            if (string.IsNullOrWhiteSpace(descricao))
                return BadRequest("Descrição do produto é obrigatória.");

            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status do produto é obrigatório.");

            var existente = await _service.Obter(codigo);
            if (existente != null)
                return Conflict($"Produto já existente com o código '{codigo}'.");

            try
            {
                var entidade = new Produto
                {
                    CodigoProduto = codigo,
                    Descricao = descricao,
                    Status = status
                };

                await _service.Incluir(entidade);

                return CreatedAtAction(nameof(GetByCodigo), new { codigo = entidade.CodigoProduto }, entidade);
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"[ERRO DB] {dbEx.InnerException?.Message ?? dbEx.Message}");
                return StatusCode(500, "Erro ao salvar produto no banco de dados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] {ex.Message}");
                return StatusCode(500, "Erro interno ao cadastrar o produto.");
            }
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="codigo">Código do produto</param>
        /// <param name="request">Dados atualizados</param>
        /// <returns>NoContent em caso de sucesso, ou erro de validação.</returns>
        [HttpPut("{codigo}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put(string codigo, [FromBody] ProdutoRequest request)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("Código do produto da URL é obrigatório.");

            if (request == null || string.IsNullOrWhiteSpace(request.CodigoProduto))
                return BadRequest("Dados do corpo da requisição são obrigatórios.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var codigoRequest = request.CodigoProduto.Trim();
            if (!codigo.Equals(codigoRequest, StringComparison.OrdinalIgnoreCase))
                return BadRequest("Código da URL deve coincidir com o do corpo.");

            var existente = await _service.Obter(codigo);
            if (existente == null)
                return NotFound($"Produto com código '{codigo}' não encontrado.");

            existente.Descricao = request.Descricao?.Trim();
            existente.Status = request.Status?.Trim().ToUpper();

            try
            {
                await _service.Atualizar(existente);
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"[ERRO DB] {dbEx.InnerException?.Message ?? dbEx.Message}");
                return StatusCode(500, "Erro ao atualizar produto no banco de dados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] {ex.Message}");
                return StatusCode(500, "Erro interno ao atualizar o produto.");
            }
        }

        /// <summary>
        /// Remove um produto pelo código.
        /// </summary>
        /// <param name="codigo">Código do produto</param>
        /// <returns>NoContent em caso de sucesso.</returns>
        [HttpDelete("{codigo}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string codigo)
        {
            await _service.Remover(codigo);
            return NoContent();
        }

        /// <summary>
        /// Retorna uma lista paginada de produtos com filtros opcionais por descrição e status.
        /// </summary>
        /// <param name="descricao">Filtro por descrição</param>
        /// <param name="status">Filtro por status</param>
        /// <param name="page">Página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <returns>Página de produtos filtrados.</returns>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<ProdutoResponse>), 200)]
        public async Task<IActionResult> GetPaginado(
            [FromQuery] string? descricao,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var filtro = PredicateBuilder.True<Produto>();

            if (!string.IsNullOrWhiteSpace(descricao))
                filtro = filtro.And(p => p.Descricao.Contains(descricao));

            if (!string.IsNullOrWhiteSpace(status))
                filtro = filtro.And(p => p.Status == status);

            var resultado = await _service.BuscarPaginado(new PagedQuery<Produto>
            {
                Filter = filtro,
                Page = page,
                PageSize = pageSize
            });

            return Ok(new PagedResult<ProdutoResponse>
            {
                Page = resultado.Page,
                PageSize = resultado.PageSize,
                TotalCount = resultado.TotalCount,
                Items = _mapper.Map<List<ProdutoResponse>>(resultado.Items)
            });
        }
    }
}
