using MovimentosManual.Domain.Entities;
using MovimentosManual.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MovimentosManual.Application.Services
{
    public class ProdutoCosifService
    {
        private readonly MovimentosDbContext _context;

        public ProdutoCosifService(MovimentosDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os produtos COSIF cadastrados.
        /// </summary>
        public async Task<List<ProdutoCosif>> ListarTodos()
        {
            return await _context.ProdutosCosif.ToListAsync();
        }

        /// <summary>
        /// Retorna um produto COSIF com base na chave composta.
        /// </summary>
        public async Task<ProdutoCosif?> Obter(string codigoProduto, string codigoCosif)
        {
            return await _context.ProdutosCosif.FindAsync(codigoProduto, codigoCosif);
        }

        /// <summary>
        /// Adiciona um novo produto COSIF ao banco de dados.
        /// </summary>
        public async Task Incluir(ProdutoCosif produtoCosif)
        {
            Validator.ValidateObject(produtoCosif, new ValidationContext(produtoCosif), validateAllProperties: true);

            // Verifica duplicidade
            var existente = await Obter(produtoCosif.CodigoProduto, produtoCosif.CodigoCosif);
            if (existente != null)
                throw new InvalidOperationException("Já existe um ProdutoCosif com os códigos informados.");

            _context.ProdutosCosif.Add(produtoCosif);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza os dados de um produto COSIF existente.
        /// </summary>
        public async Task Atualizar(ProdutoCosif produtoCosif)
        {
            Validator.ValidateObject(produtoCosif, new ValidationContext(produtoCosif), validateAllProperties: true);

            var existente = await Obter(produtoCosif.CodigoProduto, produtoCosif.CodigoCosif);
            if (existente == null)
                throw new InvalidOperationException("ProdutoCosif não encontrado.");

            existente.CodigoClassificacao = produtoCosif.CodigoClassificacao;
            existente.Status = produtoCosif.Status;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um produto COSIF, se não houver dependências em lançamentos manuais.
        /// </summary>
        public async Task Remover(string codigoProduto, string codigoCosif)
        {
            var existeMovimento = await _context.MovimentosManuais
                .AnyAsync(m => m.CodigoProduto == codigoProduto && m.CodigoCosif == codigoCosif);

            if (existeMovimento)
                throw new InvalidOperationException("Não é possível excluir. Existem lançamentos manuais vinculados a este ProdutoCosif.");

            var existente = await Obter(codigoProduto, codigoCosif);
            if (existente == null)
                throw new InvalidOperationException("ProdutoCosif não encontrado.");

            _context.ProdutosCosif.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }
}
