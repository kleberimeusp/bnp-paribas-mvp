using Microsoft.EntityFrameworkCore;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Infrastructure.Context
{
    public class MovimentosDbContext : DbContext
    {
        public MovimentosDbContext(DbContextOptions<MovimentosDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoCosif> ProdutosCosif { get; set; }
        public DbSet<MovimentoManual> MovimentosManuais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Produto
            modelBuilder.Entity<Produto>()
                .ToTable("PRODUTO", "dbo");

            // ProdutoCosif
            modelBuilder.Entity<ProdutoCosif>()
                .ToTable("PRODUTO_COSIF", "dbo")
                .HasKey(pc => new { pc.CodigoProduto, pc.CodigoCosif });

            modelBuilder.Entity<ProdutoCosif>()
                .HasOne(pc => pc.Produto)
                .WithMany(p => p.ProdutosCosif)
                .HasForeignKey(pc => pc.CodigoProduto);

            // MovimentoManual
            modelBuilder.Entity<MovimentoManual>()
                .ToTable("MOVIMENTO_MANUAL", "dbo")
                .HasKey(m => m.NumeroLancamento);

            modelBuilder.Entity<MovimentoManual>()
                .HasOne(m => m.ProdutoCosif)
                .WithMany(pc => pc.Movimentos)
                .HasForeignKey(m => new { m.CodigoProduto, m.CodigoCosif });
        }
    }
}
