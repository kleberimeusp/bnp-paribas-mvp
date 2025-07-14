// ================= Application =================
using MovimentosManual.Domain.Entities;
using MovimentosManual.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MovimentosManual.Application.Services
{
    public class MovimentoService
    {
        private readonly MovimentosDbContext _context;

        public MovimentoService(MovimentosDbContext context) => _context = context;

        public async Task<List<MovimentoManual>> ListarPorMesAno(decimal mes, decimal ano)
        {
            return await _context.MovimentosManuais
                .Where(m => m.Mes == mes && m.Ano == ano)
                .ToListAsync();
        }

        public async Task<List<MovimentoManual>> ListarTodos()
        {
            return await _context.MovimentosManuais.ToListAsync();
        }

        public async Task<MovimentoManual?> Obter(long numeroLancamento)
        {
            return await _context.MovimentosManuais.FindAsync(numeroLancamento);
        }

        public async Task Incluir(MovimentoManual movimento)
        {
            Validator.ValidateObject(movimento, new ValidationContext(movimento), validateAllProperties: true);

            movimento.NumeroLancamento = await GerarNumeroLancamento(movimento.Mes, movimento.Ano);

            _context.MovimentosManuais.Add(movimento);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(MovimentoManual movimento)
        {
            var existente = await _context.MovimentosManuais.FindAsync(movimento.NumeroLancamento);
            if (existente == null)
                throw new Exception("Movimento não encontrado.");

            existente.CodigoProduto = movimento.CodigoProduto;
            existente.CodigoCosif = movimento.CodigoCosif;
            existente.Descricao = movimento.Descricao;
            existente.Valor = movimento.Valor;

            await _context.SaveChangesAsync();
        }

        public async Task Remover(long numeroLancamento)
        {
            var existente = await _context.MovimentosManuais.FindAsync(numeroLancamento);
            if (existente == null)
                throw new Exception("Movimento não encontrado.");

            _context.MovimentosManuais.Remove(existente);
            await _context.SaveChangesAsync();
        }

        private async Task<long> GerarNumeroLancamento(decimal mes, decimal ano)
        {
            var max = await _context.MovimentosManuais
                        .Where(m => m.Mes == mes && m.Ano == ano)
                        .MaxAsync(m => (long?)m.NumeroLancamento) ?? 0;

            return max + 1;
        }
    }
}
