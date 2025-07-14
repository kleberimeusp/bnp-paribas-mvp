using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Application.Services;
using MovimentosManual.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ProdutosCosifController : ControllerBase
{
    private readonly ProdutoCosifService _service;

    public ProdutosCosifController(ProdutoCosifService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProdutoCosif>>> Get()
    {
        return await _service.ListarTodos();
    }

    [HttpGet("{codigoProduto}/{codigoCosif}")]
    public async Task<ActionResult<ProdutoCosif>> Get(string codigoProduto, string codigoCosif)
    {
        var item = await _service.Obter(codigoProduto, codigoCosif);
        if (item == null) return NotFound();
        return item;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProdutoCosif produtoCosif)
    {
        await _service.Incluir(produtoCosif);
        return CreatedAtAction(nameof(Get), new { codigoProduto = produtoCosif.CodigoProduto, codigoCosif = produtoCosif.CodigoCosif }, produtoCosif);
    }

    [HttpPut("{codigoProduto}/{codigoCosif}")]
    public async Task<ActionResult> Put(string codigoProduto, string codigoCosif, [FromBody] ProdutoCosif produtoCosif)
    {
        if (codigoProduto != produtoCosif.CodigoProduto || codigoCosif != produtoCosif.CodigoCosif) return BadRequest();
        await _service.Atualizar(produtoCosif);
        return NoContent();
    }

    [HttpDelete("{codigoProduto}/{codigoCosif}")]
    public async Task<ActionResult> Delete(string codigoProduto, string codigoCosif)
    {
        await _service.Remover(codigoProduto, codigoCosif);
        return NoContent();
    }
}
