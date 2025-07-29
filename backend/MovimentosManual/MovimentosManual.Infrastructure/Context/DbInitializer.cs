using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovimentosManual.Domain.Entities;
using System;
using System.Linq;

namespace MovimentosManual.Infrastructure.Context
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = new MovimentosDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<MovimentosDbContext>>());

            context.Database.EnsureCreated();

            // Evita seed duplicado
            if (context.Produtos.Any() || context.Cosifs.Any() || context.ProdutosCosif.Any() || context.MovimentosManuais.Any())
                return;

            // Dados iniciais
            var produto = new Produto
            {
                CodigoProduto = "P001",
                Descricao = "Produto 1",
                Status = "A"
            };

            var cosif = new Cosif
            {
                CodigoCosif = "COSIF001",
                Descricao = "Conta COSIF 1",
                Status = "A"
            };

            var produtoCosif = new ProdutoCosif
            {
                CodigoProduto = "P001",
                CodigoCosif = "COSIF001",
                CodigoClassificacao = "CLS001",
                Status = "A",
                Produto = produto,
                Cosif = cosif
            };

            var movimento = new MovimentoManual
            {
                Mes = 7,
                Ano = 2025,
                NumeroLancamento = 1,
                CodigoProduto = "P001",
                CodigoCosif = "COSIF001",
                Descricao = "Teste Seed",
                CodigoUsuario = "admin",
                DataMovimento = DateTime.Now,
                Valor = 123.45M
            };

            // Inserir dados
            context.Produtos.Add(produto);
            context.Cosifs.Add(cosif);
            context.ProdutosCosif.Add(produtoCosif);
            context.MovimentosManuais.Add(movimento);

            context.SaveChanges();
        }
    }
}
