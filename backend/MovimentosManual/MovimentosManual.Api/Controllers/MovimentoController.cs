using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Application.Services;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly MovimentoService _service;

        public MovimentoController(MovimentoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os movimentos manuais.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var lista = await _service.ListarTodos();
            return Ok(lista);
        }

        /// <summary>
        /// Lista movimentos por mês e ano.
        /// </summary>
        [HttpGet("por-data")]
        public async Task<IActionResult> GetPorData([FromQuery] int mes, [FromQuery] int ano)
        {
            if (mes is < 1 or > 12 || ano < 1900)
                return BadRequest("Mês ou ano inválido.");

            var lista = await _service.ListarPorMesAno(mes, ano);
            return Ok(lista);
        }

        /// <summary>
        /// Obtém um movimento por número de lançamento.
        /// </summary>
        [HttpGet("{numeroLancamento:long}")]
        public async Task<IActionResult> GetById(long numeroLancamento)
        {
            var mov = await _service.Obter(numeroLancamento);
            return mov is null ? NotFound() : Ok(mov);
        }

        /// <summary>
        /// Cria um novo movimento manual.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovimentoManual movimento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.Incluir(movimento);
            return CreatedAtAction(nameof(GetById), new { numeroLancamento = movimento.NumeroLancamento }, movimento);
        }

        /// <summary>
        /// Atualiza um movimento manual existente.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MovimentoManual movimento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.Atualizar(movimento);
            return NoContent();
        }

        /// <summary>
        /// Remove um movimento por número de lançamento.
        /// </summary>
        [HttpDelete("por-lancamento/{numeroLancamento:long}")]
        public async Task<IActionResult> DeletePorNumero(long numeroLancamento)
        {
            await _service.Remover(numeroLancamento);
            return NoContent();
        }

        /// <summary>
        /// Remove um movimento por produto e código COSIF.
        /// </summary>
        [HttpDelete("por-produto-cosif/{codigoProduto}/{codigoCosif}")]
        public async Task<IActionResult> DeletePorProdutoCosif(string codigoProduto, string codigoCosif)
        {
            await _service.Remover(codigoProduto, codigoCosif);
            return NoContent();
        }
    }
}
