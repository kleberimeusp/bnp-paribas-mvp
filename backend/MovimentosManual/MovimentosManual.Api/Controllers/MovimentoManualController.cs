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
    [Produces("application/json")]
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
        /// <returns>Lista de lançamentos manuais.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovimentoManualResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var entidades = await _movimentoService.ListarTodos();
            return Ok(_mapper.Map<IEnumerable<MovimentoManualResponse>>(entidades));
        }

        /// <summary>
        /// Consulta um lançamento manual por chave composta (mês, ano e número).
        /// </summary>
        /// <param name="mes">Mês do lançamento</param>
        /// <param name="ano">Ano do lançamento</param>
        /// <param name="numero">Número do lançamento</param>
        /// <returns>Dados do lançamento encontrado.</returns>
        [HttpGet("{mes:int}/{ano:int}/{numero:long}")]
        [ProducesResponseType(typeof(MovimentoManualResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int mes, int ano, long numero)
        {
            var item = await _movimentoService.Obter(mes, ano, numero);
            return item is null ? NotFound() : Ok(_mapper.Map<MovimentoManualResponse>(item));
        }

        /// <summary>
        /// Consulta paginada de lançamentos manuais com filtros e ordenação.
        /// </summary>
        /// <param name="request">Parâmetros de paginação e ordenação</param>
        /// <returns>Lista paginada de lançamentos manuais.</returns>
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

        /// <summary>
        /// Cria um novo lançamento manual.
        /// </summary>
        /// <param name="request">Dados do lançamento manual</param>
        /// <returns>O lançamento criado.</returns>
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

        /// <summary>
        /// Atualiza um lançamento manual existente.
        /// </summary>
        /// <param name="mes">Mês do lançamento</param>
        /// <param name="ano">Ano do lançamento</param>
        /// <param name="numero">Número do lançamento</param>
        /// <param name="request">Dados atualizados</param>
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

        /// <summary>
        /// Remove um lançamento manual por chave composta.
        /// </summary>
        /// <param name="mes">Mês do lançamento</param>
        /// <param name="ano">Ano do lançamento</param>
        /// <param name="numero">Número do lançamento</param>
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
