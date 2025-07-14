using MovimentosManual.Domain.Entities;
using MovimentosManual.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class ProdutoService
{
    private readonly MovimentosDbContext _context;

    public ProdutoService(MovimentosDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> ListarTodos()
    {
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produto?> Obter(string codigo)
    {
        return await _context.Produtos.FindAsync(codigo);
    }

    public async Task Incluir(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
    }

    public async Task Atualizar(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task Remover(string codigo)
    {
        var existente = await _context.Produtos.FindAsync(codigo);
        if (existente != null)
        {
            _context.Produtos.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}
