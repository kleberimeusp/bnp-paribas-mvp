// ================================
// MovimentosDbContext.cs
// ================================
using Microsoft.EntityFrameworkCore;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Infrastructure.Context
{
    public class MovimentosDbContext : DbContext
    {
        public MovimentosDbContext(DbContextOptions<MovimentosDbContext> options)
            : base(options) { }

        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<Cosif> Cosifs { get; set; } = null!;
        public DbSet<ProdutoCosif> ProdutosCosif { get; set; } = null!;
        public DbSet<MovimentoManual> MovimentosManuais { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== PRODUTO ==========
            modelBuilder.Entity<Produto>()
                .ToTable("PRODUTO", "dbo")
                .HasKey(p => p.CodigoProduto);

            modelBuilder.Entity<Produto>()
                .Property(p => p.CodigoProduto).HasMaxLength(4);
            modelBuilder.Entity<Produto>()
                .Property(p => p.Descricao).HasMaxLength(30);
            modelBuilder.Entity<Produto>()
                .Property(p => p.Status).HasMaxLength(1);

            // ========== COSIF ==========
            modelBuilder.Entity<Cosif>()
                .ToTable("COSIF", "dbo")
                .HasKey(c => c.CodigoCosif);

            modelBuilder.Entity<Cosif>()
                .Property(c => c.CodigoCosif).HasMaxLength(11);
            modelBuilder.Entity<Cosif>()
                .Property(c => c.Descricao).HasMaxLength(50);
            modelBuilder.Entity<Cosif>()
                .Property(c => c.Status).HasMaxLength(1);

            // ========== PRODUTO_COSIF ==========
            modelBuilder.Entity<ProdutoCosif>()
                .ToTable("PRODUTO_COSIF", "dbo")
                .HasKey(pc => new { pc.CodigoProduto, pc.CodigoCosif });

            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.CodigoProduto).HasMaxLength(4);
            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.CodigoCosif).HasMaxLength(11);
            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.CodigoClassificacao).HasMaxLength(6);
            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.Status).HasMaxLength(1);

            modelBuilder.Entity<ProdutoCosif>()
                .HasOne(pc => pc.Produto)
                .WithMany(p => p.ProdutosCosif)
                .HasForeignKey(pc => pc.CodigoProduto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProdutoCosif>()
                .HasOne(pc => pc.Cosif)
                .WithMany(c => c.ProdutosCosif)
                .HasForeignKey(pc => pc.CodigoCosif)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== MOVIMENTO_MANUAL ==========
            modelBuilder.Entity<MovimentoManual>()
                .ToTable("MOVIMENTO_MANUAL", "dbo")
                .HasKey(m => new { m.Mes, m.Ano, m.NumeroLancamento });

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Mes)
                .HasColumnName("DAT_MES")
                .HasColumnType("int");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Ano)
                .HasColumnName("DAT_ANO")
                .HasColumnType("int");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.NumeroLancamento)
                .HasColumnName("NUM_LANCAMENTO")
                .HasColumnType("int");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.DataMovimento)
                .HasColumnName("DAT_MOVIMENTO")
                .HasColumnType("smalldatetime");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.CodigoProduto).HasMaxLength(4).HasColumnName("COD_PRODUTO");
            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.CodigoCosif).HasMaxLength(11).HasColumnName("COD_COSIF");
            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Descricao).HasMaxLength(50).HasColumnName("DES_DESCRICAO");
            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.CodigoUsuario).HasMaxLength(15).HasColumnName("COD_USUARIO");
            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Valor).HasColumnType("decimal(18,2)").HasColumnName("VAL_VALOR");

            modelBuilder.Entity<MovimentoManual>()
                .HasOne(m => m.Produto)
                .WithMany(p => p.Movimentos)
                .HasForeignKey(m => m.CodigoProduto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimentoManual>()
                .HasOne(m => m.Cosif)
                .WithMany(c => c.Movimentos)
                .HasForeignKey(m => m.CodigoCosif)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}