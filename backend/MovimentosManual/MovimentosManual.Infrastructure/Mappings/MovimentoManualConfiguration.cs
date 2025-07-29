using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Infrastructure.Mappings
{
    public class MovimentoManualConfiguration : IEntityTypeConfiguration<MovimentoManual>
    {
        public void Configure(EntityTypeBuilder<MovimentoManual> builder)
        {
            builder.ToTable("MOVIMENTO_MANUAL", "dbo");

            // Chave composta: COD_PRODUTO + COD_COSIF + NUM_LANCAMENTO + DAT_ANO + DAT_MES
            builder.HasKey(m => new
            {
                m.CodigoProduto,
                m.CodigoCosif,
                m.NumeroLancamento,
                m.Ano,
                m.Mes
            });

            builder.Property(m => m.DataMovimento)
                   .HasColumnName("DAT_MOVIMENTO")
                   .IsRequired();

            builder.Property(m => m.CodigoProduto)
                   .HasColumnName("COD_PRODUTO")
                   .HasMaxLength(4)
                   .IsRequired();

            builder.Property(m => m.CodigoCosif)
                   .HasColumnName("COD_COSIF")
                   .HasMaxLength(11)
                   .IsRequired();

            builder.Property(m => m.Descricao)
                   .HasColumnName("DES_DESCRICAO")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(m => m.Valor)
                   .HasColumnName("VAL_VALOR")
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(m => m.NumeroLancamento)
                   .HasColumnName("NUM_LANCAMENTO")
                   .IsRequired();

            builder.Property(m => m.Mes)
                   .HasColumnName("DAT_MES")
                   .IsRequired();

            builder.Property(m => m.Ano)
                   .HasColumnName("DAT_ANO")
                   .IsRequired();

            builder.Property(m => m.CodigoUsuario)
                   .HasColumnName("COD_USUARIO")
                   .HasMaxLength(15)
                   .IsRequired();

            // Relacionamentos
            builder.HasOne(m => m.Produto)
                   .WithMany()
                   .HasForeignKey(m => m.CodigoProduto)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Cosif)
                   .WithMany()
                   .HasForeignKey(m => m.CodigoCosif)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
