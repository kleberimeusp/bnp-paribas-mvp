using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Common;
using MovimentosManual.Core.Pagination;
using AutoMapper;
using System.Linq.Expressions;
using MovimentosManual.Infrastructure.Common.Linq;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Application.Models.Filter;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CosifController : ControllerBase
    {
        private readonly ICosifService _cosifService;
        private readonly IMapper _mapper;

        public CosifController(ICosifService cosifService, IMapper mapper)
        {
            _cosifService = cosifService;
            _mapper = mapper;
        }

        #region GETs

        /// <summary>
        /// Retorna todos os registros Cosif.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CosifResponse>), 200)]
        public async Task<ActionResult<IEnumerable<CosifResponse>>> GetAll()
        {
            var entidades = await _cosifService.ListarTodos();
            return Ok(_mapper.Map<IEnumerable<CosifResponse>>(entidades));
        }

        /// <summary>
        /// Retorna um registro Cosif por código.
        /// </summary>
        /// <param name="codigo">Código Cosif</param>
        [HttpGet("{codigo}")]
        [ProducesResponseType(typeof(CosifResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CosifResponse>> GetByCodigo(string codigo)
        {
            var entidade = await _cosifService.ObterPorCodigo(codigo);
            if (entidade is null) return NotFound();

            return Ok(_mapper.Map<CosifResponse>(entidade));
        }

        /// <summary>
        /// Retorna uma lista paginada de registros Cosif com filtros e ordenação.
        /// </summary>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<CosifResponse>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestWithSort<CosifFilter> request)
        {
            var filtro = BuildFilter(request.Filters);
            var ordenacoes = BuildOrdering(request);

            var resultado = await _cosifService.BuscarPaginado(new PagedQuery<Cosif>
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Filter = filtro,
                Orderings = ordenacoes
            });

            return Ok(new PagedResult<CosifResponse>
            {
                Page = resultado.Page,
                PageSize = resultado.PageSize,
                TotalCount = resultado.TotalCount,
                Items = _mapper.Map<List<CosifResponse>>(resultado.Items)
            });
        }

        #endregion

        #region POST/PUT/DELETE

        /// <summary>
        /// Cria um novo registro Cosif.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CosifResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CosifRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entidade = _mapper.Map<Cosif>(request);
            await _cosifService.Incluir(entidade);

            var dto = _mapper.Map<CosifResponse>(entidade);
            return CreatedAtAction(nameof(GetByCodigo), new { codigo = entidade.CodigoCosif }, dto);
        }

        /// <summary>
        /// Atualiza um registro Cosif existente.
        /// </summary>
        /// <param name="codigo">Código Cosif</param>
        [HttpPut("{codigo}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(string codigo, [FromBody] CosifRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (codigo != request.CodigoCosif)
                return BadRequest("O código da rota e do corpo devem ser iguais.");

            var entidade = _mapper.Map<Cosif>(request);
            await _cosifService.Atualizar(entidade);
            return NoContent();
        }

        /// <summary>
        /// Remove um registro Cosif pelo código.
        /// </summary>
        /// <param name="codigo">Código Cosif</param>
        [HttpDelete("{codigo}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string codigo)
        {
            await _cosifService.Remover(codigo);
            return NoContent();
        }

        #endregion

        #region Helpers

        private static Expression<Func<Cosif, bool>> BuildFilter(CosifFilter? filterDto)
        {
            var filtro = PredicateBuilder.True<Cosif>();

            if (filterDto == null) return filtro;

            if (!string.IsNullOrWhiteSpace(filterDto.CodigoCosif))
                filtro = filtro.And(c => c.CodigoCosif == filterDto.CodigoCosif);

            if (!string.IsNullOrWhiteSpace(filterDto.Descricao))
                filtro = filtro.And(c => c.Descricao.Contains(filterDto.Descricao));

            if (!string.IsNullOrWhiteSpace(filterDto.Status))
                filtro = filtro.And(c => c.Status == filterDto.Status);

            return filtro;
        }

        private static List<OrderClause<Cosif>> BuildOrdering(PagedRequestWithSort<CosifFilter> request)
        {
            var ordenacoes = new List<OrderClause<Cosif>>();

            var camposOrdenacao = new Dictionary<string, Expression<Func<Cosif, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                { "CodigoCosif", c => c.CodigoCosif },
                { "Descricao", c => c.Descricao },
                { "Status", c => c.Status },
            };

            if (!string.IsNullOrWhiteSpace(request.OrderBy) &&
                camposOrdenacao.TryGetValue(request.OrderBy, out var campo))
            {
                ordenacoes.Add(new OrderClause<Cosif>(campo, request.Descending));
            }

            return ordenacoes;
        }

        #endregion
    }
}
