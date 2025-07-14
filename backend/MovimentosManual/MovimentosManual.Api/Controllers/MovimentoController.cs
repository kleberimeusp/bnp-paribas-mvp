using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Application.Services;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Api.Controllers
{

    [ApiController]
    [Route("api/movimentos")]
    public class MovimentoController : ControllerBase
    {
        private readonly MovimentoService _service;

        public MovimentoController(MovimentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var lista = await _service.ListarTodos();
            return Ok(lista);
        }

        [HttpGet("por-data")]
        public async Task<IActionResult> GetPorData([FromQuery] int mes, [FromQuery] int ano)
        {
            if (mes < 1 || mes > 12 || ano < 1900)
                return BadRequest("Mês ou ano inválido.");

            var lista = await _service.ListarPorMesAno(mes, ano);
            return Ok(lista);
        }

        [HttpGet("{numeroLancamento:long}")]
        public async Task<IActionResult> GetById(long numeroLancamento)
        {
            var mov = await _service.Obter(numeroLancamento);
            if (mov == null) return NotFound();
            return Ok(mov);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovimentoManual movimento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.Incluir(movimento);
            return Created(string.Empty, movimento);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MovimentoManual movimento)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.Atualizar(movimento);
            return NoContent();
        }

        [HttpDelete("{numeroLancamento:long}")]
        public async Task<IActionResult> Delete(long numeroLancamento)
        {
            await _service.Remover(numeroLancamento);
            return NoContent();
        }
    }
} 
