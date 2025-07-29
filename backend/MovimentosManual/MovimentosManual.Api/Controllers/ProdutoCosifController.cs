using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Infrastructure.Common.Linq;
using MovimentosManual.Core.Pagination;
using AutoMapper;
using MovimentosManual.Core.Common;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProdutoCosifController : ControllerBase
    {
        private readonly IProdutoCosifService _service;
        private readonly IMapper _mapper;

        public ProdutoCosifController(IProdutoCosifService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        #region GET

        /// <summary>
        /// Retorna todos os vínculos entre Produto e Cosif.
        /// </summary>
        /// <returns>Lista de ProdutoCosif.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProdutoCosifResponse>), 200)]
        public async Task<ActionResult<List<ProdutoCosifResponse>>> GetAll()
        {
            var entidades = await _service.ListarTodos();
            return Ok(_mapper.Map<List<ProdutoCosifResponse>>(entidades));
        }

        /// <summary>
        /// Retorna um vínculo Produto x Cosif por identificadores.
        /// </summary>
        /// <param name="codigoProduto">Código do produto</param>
        /// <param name="codigoCosif">Código do cosif</param>
        /// <returns>Objeto ProdutoCosif correspondente.</returns>
        [HttpGet("{codigoProduto}/{codigoCosif}")]
        [ProducesResponseType(typeof(ProdutoCosifResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProdutoCosifResponse>> GetById(string codigoProduto, string codigoCosif)
        {
            var entidade = await _service.Obter(codigoProduto, codigoCosif);
            return entidade is null ? NotFound() : Ok(_mapper.Map<ProdutoCosifResponse>(entidade));
        }

        #endregion

        #region POST / PUT / DELETE

        /// <summary>
        /// Cria um novo vínculo Produto x Cosif.
        /// </summary>
        /// <param name="request">Dados do vínculo</param>
        /// <returns>Objeto criado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProdutoCosifResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] ProdutoCosifRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entidade = _mapper.Map<ProdutoCosif>(request);
            await _service.Incluir(entidade);

            var response = _mapper.Map<ProdutoCosifResponse>(entidade);
            return CreatedAtAction(nameof(GetById), new
            {
                codigoProduto = entidade.CodigoProduto,
                codigoCosif = entidade.CodigoCosif
            }, response);
        }

        /// <summary>
        /// Atualiza um vínculo existente Produto x Cosif.
        /// </summary>
        /// <param name="codigoProduto">Código do produto</param>
        /// <param name="codigoCosif">Código do cosif</param>
        /// <param name="request">Dados atualizados</param>
        /// <returns>NoContent em caso de sucesso.</returns>
        [HttpPut("{codigoProduto}/{codigoCosif}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(string codigoProduto, string codigoCosif, [FromBody] ProdutoCosifRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (codigoProduto != request.CodigoProduto || codigoCosif != request.CodigoCosif)
                return BadRequest("Os códigos da URL e do corpo da requisição devem coincidir.");

            var entidade = _mapper.Map<ProdutoCosif>(request);
            await _service.Atualizar(entidade);
            return NoContent();
        }

        /// <summary>
        /// Remove um vínculo Produto x Cosif.
        /// </summary>
        /// <param name="codigoProduto">Código do produto</param>
        /// <param name="codigoCosif">Código do cosif</param>
        /// <returns>NoContent se removido.</returns>
        [HttpDelete("{codigoProduto}/{codigoCosif}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string codigoProduto, string codigoCosif)
        {
            await _service.Remover(codigoProduto, codigoCosif);
            return NoContent();
        }

        #endregion

        #region PAGINAÇÃO

        /// <summary>
        /// Retorna uma lista paginada de vínculos Produto x Cosif com filtros opcionais.
        /// </summary>
        /// <param name="status">Filtro por status</param>
        /// <param name="produto">Filtro por descrição do produto</param>
        /// <param name="cosif">Filtro por código Cosif</param>
        /// <param name="page">Página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <returns>Lista paginada de ProdutoCosifResponse</returns>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<ProdutoCosifResponse>), 200)]
        public async Task<IActionResult> GetPaginado(
            [FromQuery] string? status,
            [FromQuery] string? produto,
            [FromQuery] string? cosif,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var filtro = PredicateBuilder.True<ProdutoCosif>();

            if (!string.IsNullOrWhiteSpace(produto))
                filtro = filtro.And(p => p.Produto.Descricao.Contains(produto));

            if (!string.IsNullOrWhiteSpace(status))
                filtro = filtro.And(p => p.Status == status);

            if (!string.IsNullOrWhiteSpace(cosif))
                filtro = filtro.And(p => p.CodigoCosif.Contains(cosif));

            var resultado = await _service.BuscarPaginado(new PagedQuery<ProdutoCosif>
            {
                Filter = filtro,
                Page = page,
                PageSize = pageSize
            });

            return Ok(new PagedResult<ProdutoCosifResponse>
            {
                Page = resultado.Page,
                PageSize = resultado.PageSize,
                TotalCount = resultado.TotalCount,
                Items = _mapper.Map<List<ProdutoCosifResponse>>(resultado.Items)
            });
        }

        #endregion
    }
}
