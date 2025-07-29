// ================================
// MovimentoManualController.cs
// ================================
using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Core.Pagination;
using AutoMapper;
using MovimentosManual.Core.Common;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoManualController : ControllerBase
    {
        private readonly IMovimentoManualService _movimentoService;
        private readonly IMapper _mapper;

        public MovimentoManualController(
            IMovimentoManualService movimentoService,
            IMapper mapper)
        {
            _movimentoService = movimentoService;
            _mapper = mapper;
        }

        #region GETs

        /// <summary>
        /// Lista todos os lançamentos manuais.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovimentoManualResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var entidades = await _movimentoService.ListarTodos();
            return Ok(_mapper.Map<IEnumerable<MovimentoManualResponse>>(entidades));
        }

        /// <summary>
        /// Consulta lançamento manual por chave composta.
        /// </summary>
        [HttpGet("{mes:int}/{ano:int}/{numero:long}")]
        [ProducesResponseType(typeof(MovimentoManualResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int mes, int ano, long numero)
        {
            var item = await _movimentoService.Obter(mes, ano, numero);
            return item is null ? NotFound() : Ok(_mapper.Map<MovimentoManualResponse>(item));
        }

        /// <summary>
        /// Consulta paginada com filtros e ordenação avançada.
        /// </summary>
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<MovimentoManualResponse>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery] PagedRequestWithSort<MovimentoManual> request)
        {
            var resultado = await _movimentoService.GetPagedAsync(request);

            return Ok(new PagedResult<MovimentoManualResponse>
            {
                Page = resultado.Page,
                PageSize = resultado.PageSize,
                TotalCount = resultado.TotalCount,
                Items = _mapper.Map<List<MovimentoManualResponse>>(resultado.Items)
            });
        }

        #endregion

        #region POST / PUT / DELETE

        [HttpPost]
        [ProducesResponseType(typeof(MovimentoManualResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] MovimentoManualRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entidade = _mapper.Map<MovimentoManual>(request);
            await _movimentoService.Incluir(entidade);

            return CreatedAtAction(nameof(GetById), new
            {
                mes = entidade.Mes,
                ano = entidade.Ano,
                numero = entidade.NumeroLancamento
            }, _mapper.Map<MovimentoManualResponse>(entidade));
        }

        [HttpPut("{mes:int}/{ano:int}/{numero:long}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int mes, int ano, long numero, [FromBody] MovimentoManualRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (mes != request.Mes || ano != request.Ano || numero != request.NumeroLancamento)
                return BadRequest("Parâmetros de rota e do corpo devem coincidir.");

            var entidade = _mapper.Map<MovimentoManual>(request);
            await _movimentoService.Atualizar(entidade);

            return NoContent();
        }

        [HttpDelete("{mes:int}/{ano:int}/{numero:long}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int mes, int ano, long numero)
        {
            await _movimentoService.Remover(mes, ano, numero);
            return NoContent();
        }

        #endregion
    }
}