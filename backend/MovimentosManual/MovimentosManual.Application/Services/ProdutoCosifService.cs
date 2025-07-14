using MovimentosManual.Domain.Entities;
using MovimentosManual.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class ProdutoCosifService
{
    private readonly MovimentosDbContext _context;

    public ProdutoCosifService(MovimentosDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProdutoCosif>> ListarTodos()
    {
        return await _context.ProdutosCosif.ToListAsync();
    }

    public async Task<ProdutoCosif?> Obter(string codigoProduto, string codigoCosif)
    {
        return await _context.ProdutosCosif
            .FindAsync(codigoProduto, codigoCosif);
    }

    public async Task Incluir(ProdutoCosif produtoCosif)
    {
        _context.ProdutosCosif.Add(produtoCosif);
        await _context.SaveChangesAsync();
    }

    public async Task Atualizar(ProdutoCosif produtoCosif)
    {
        _context.ProdutosCosif.Update(produtoCosif);
        await _context.SaveChangesAsync();
    }

    public async Task Remover(string codigoProduto, string codigoCosif)
    {
        var existente = await _context.ProdutosCosif.FindAsync(codigoProduto, codigoCosif);
        if (existente != null)
        {
            _context.ProdutosCosif.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}
