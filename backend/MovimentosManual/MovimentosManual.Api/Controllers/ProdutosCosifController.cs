using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Application.Services;
using MovimentosManual.Application.DTOs;
using MovimentosManual.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosCosifController : ControllerBase
    {
        private readonly ProdutoCosifService _service;

        public ProdutosCosifController(ProdutoCosifService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os registros de ProdutoCosif.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ProdutoCosif>>> Get()
        {
            var lista = await _service.ListarTodos();
            return Ok(lista);
        }

        /// <summary>
        /// Obtém um ProdutoCosif por código de produto e código cosif.
        /// </summary>
        [HttpGet("{codigoProduto}/{codigoCosif}")]
        public async Task<ActionResult<ProdutoCosif>> Get(string codigoProduto, string codigoCosif)
        {
            var item = await _service.Obter(codigoProduto, codigoCosif);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>
        /// Cria um novo ProdutoCosif.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoCosifDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var produtoCosif = new ProdutoCosif
                {
                    CodigoProduto = dto.CodigoProduto,
                    CodigoCosif = dto.CodigoCosif,
                    CodigoClassificacao = dto.CodigoClassificacao,
                    Status = dto.Status
                };

                await _service.Incluir(produtoCosif);

                return CreatedAtAction(nameof(Get), new
                {
                    codigoProduto = produtoCosif.CodigoProduto,
                    codigoCosif = produtoCosif.CodigoCosif
                }, produtoCosif);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro ao criar ProdutoCosif: {ex.Message}" });
            }
        }

        /// <summary>
        /// Atualiza um ProdutoCosif existente.
        /// </summary>
        [HttpPut("{codigoProduto}/{codigoCosif}")]
        public async Task<ActionResult> Put(string codigoProduto, string codigoCosif, [FromBody] ProdutoCosif produtoCosif)
        {
            if (codigoProduto != produtoCosif.CodigoProduto || codigoCosif != produtoCosif.CodigoCosif)
                return BadRequest(new { erro = "Chaves primárias inconsistentes com o corpo da requisição." });

            try
            {
                await _service.Atualizar(produtoCosif);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro ao atualizar ProdutoCosif: {ex.Message}" });
            }
        }

        /// <summary>
        /// Remove um ProdutoCosif pelo código de produto e código cosif.
        /// </summary>
        [HttpDelete("{codigoProduto}/{codigoCosif}")]
        public async Task<ActionResult> Delete(string codigoProduto, string codigoCosif)
        {
            try
            {
                await _service.Remover(codigoProduto, codigoCosif);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro ao remover ProdutoCosif: {ex.Message}" });
            }
        }
    }
}
