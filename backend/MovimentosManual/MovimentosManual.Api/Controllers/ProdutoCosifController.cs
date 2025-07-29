using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Infrastructure.Common.Linq;
using MovimentosManual.Core.Pagination; // Corrigido
using AutoMapper;
using MovimentosManual.Core.Common;
using MovimentosManual.Application.Models.Response;
using MovimentosManual.Application.Models.Request;

namespace MovimentosManual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<List<ProdutoCosifResponse>>> GetAll()
        {
            var entidades = await _service.ListarTodos();
            return Ok(_mapper.Map<List<ProdutoCosifResponse>>(entidades));
        }

        [HttpGet("{codigoProduto}/{codigoCosif}")]
        public async Task<ActionResult<ProdutoCosifResponse>> GetById(string codigoProduto, string codigoCosif)
        {
            var entidade = await _service.Obter(codigoProduto, codigoCosif);
            return entidade is null ? NotFound() : Ok(_mapper.Map<ProdutoCosifResponse>(entidade));
        }

        #endregion

        #region POST / PUT / DELETE

        [HttpPost]
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

        [HttpPut("{codigoProduto}/{codigoCosif}")]
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

        [HttpDelete("{codigoProduto}/{codigoCosif}")]
        public async Task<IActionResult> Delete(string codigoProduto, string codigoCosif)
        {
            await _service.Remover(codigoProduto, codigoCosif);
            return NoContent();
        }

        #endregion

        #region PAGINAÇÃO

        [HttpGet("paged")]
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