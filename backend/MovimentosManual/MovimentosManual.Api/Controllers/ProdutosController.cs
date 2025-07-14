using Microsoft.AspNetCore.Mvc;
using MovimentosManual.Application.Services;
using MovimentosManual.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _service;

    public ProdutosController(ProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Produto>>> Get()
    {
        return await _service.ListarTodos();
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<Produto>> Get(string codigo)
    {
        var produto = await _service.Obter(codigo);
        if (produto == null) return NotFound();
        return produto;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Produto produto)
    {
        await _service.Incluir(produto);
        return CreatedAtAction(nameof(Get), new { codigo = produto.CodigoProduto }, produto);
    }

    [HttpPut("{codigo}")]
    public async Task<ActionResult> Put(string codigo, [FromBody] Produto produto)
    {
        if (codigo != produto.CodigoProduto) return BadRequest();
        await _service.Atualizar(produto);
        return NoContent();
    }

    [HttpDelete("{codigo}")]
    public async Task<ActionResult> Delete(string codigo)
    {
        await _service.Remover(codigo);
        return NoContent();
    }
}
