using Microsoft.EntityFrameworkCore;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Infrastructure.Context
{
    public class MovimentosDbContext : DbContext
    {
        public MovimentosDbContext(DbContextOptions<MovimentosDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<ProdutoCosif> ProdutosCosif { get; set; } = null!;
        public DbSet<MovimentoManual> MovimentosManuais { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= PRODUTO =================
            modelBuilder.Entity<Produto>()
                .ToTable("PRODUTO", "dbo")
                .HasKey(p => p.CodigoProduto);

            modelBuilder.Entity<Produto>()
                .Property(p => p.CodigoProduto)
                .HasColumnName("COD_PRODUTO")
                .HasMaxLength(20);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Descricao)
                .HasColumnName("DES_PRODUTO")
                .HasMaxLength(100);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Status)
                .HasColumnName("STA_STATUS")
                .HasMaxLength(1);

            // ================= PRODUTO_COSIF =================
            modelBuilder.Entity<ProdutoCosif>()
                .ToTable("PRODUTO_COSIF", "dbo")
                .HasKey(pc => new { pc.CodigoProduto, pc.CodigoCosif });

            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.CodigoProduto)
                .HasColumnName("COD_PRODUTO")
                .HasMaxLength(20);

            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.CodigoCosif)
                .HasColumnName("COD_COSIF")
                .HasMaxLength(20);

            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.CodigoClassificacao)
                .HasColumnName("COD_CLASSIFICACAO")
                .HasMaxLength(20);

            modelBuilder.Entity<ProdutoCosif>()
                .Property(pc => pc.Status)
                .HasColumnName("STA_STATUS")
                .HasMaxLength(1);

            modelBuilder.Entity<ProdutoCosif>()
                .HasOne(pc => pc.Produto)
                .WithMany(p => p.ProdutosCosif)
                .HasForeignKey(pc => pc.CodigoProduto)
                .OnDelete(DeleteBehavior.Cascade);

            // ================= MOVIMENTO_MANUAL =================
            modelBuilder.Entity<MovimentoManual>()
                .ToTable("MOVIMENTO_MANUAL", "dbo")
                .HasKey(m => m.NumeroLancamento);

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.NumeroLancamento)
                .HasColumnName("NUM_LANCAMENTO");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Mes)
                .HasColumnName("DAT_MES");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Ano)
                .HasColumnName("DAT_ANO");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.CodigoProduto)
                .HasColumnName("COD_PRODUTO")
                .HasMaxLength(20);

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.CodigoCosif)
                .HasColumnName("COD_COSIF")
                .HasMaxLength(20);

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Descricao)
                .HasColumnName("DES_DESCRICAO")
                .HasMaxLength(100);

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.CodigoUsuario)
                .HasColumnName("COD_USUARIO")
                .HasMaxLength(20);

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.DataMovimento)
                .HasColumnName("DAT_MOVIMENTO");

            modelBuilder.Entity<MovimentoManual>()
                .Property(m => m.Valor)
                .HasColumnName("VAL_VALOR");

            modelBuilder.Entity<MovimentoManual>()
                .HasOne(m => m.ProdutoCosif)
                .WithMany(pc => pc.Movimentos)
                .HasForeignKey(m => new { m.CodigoProduto, m.CodigoCosif })
                .OnDelete(DeleteBehavior.Restrict); // impede exclusão com movimentos vinculados
        }
    }
}
