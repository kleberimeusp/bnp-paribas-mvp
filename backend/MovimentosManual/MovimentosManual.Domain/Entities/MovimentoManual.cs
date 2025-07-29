using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimentosManual.Domain.Entities
{
    [Table("MOVIMENTO_MANUAL", Schema = "dbo")]
    public class MovimentoManual
    {
        // A tabela usa chave composta, você deve mapear via Fluent API — então remova o [Key]
        [NotMapped]
        public int Id { get; set; }  // Se quiser um ID interno na app, use NotMapped

        [Column("DAT_MOVIMENTO")]
        public DateTime DataMovimento { get; set; }

        [Column("COD_PRODUTO")]
        [StringLength(4)]
        public string CodigoProduto { get; set; } = string.Empty;

        [Column("COD_COSIF")]
        [StringLength(11)]
        public string CodigoCosif { get; set; } = string.Empty;

        [Column("DES_DESCRICAO")]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        [Column("VAL_VALOR")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Column("NUM_LANCAMENTO")]
        public int NumeroLancamento { get; set; }

        [Column("DAT_MES")]
        public int Mes { get; set; }     // Corrigido: DAT_MES no banco

        [Column("DAT_ANO")]
        public int Ano { get; set; }     // Corrigido: DAT_ANO no banco

        [Column("COD_USUARIO")]
        [StringLength(15)]
        public string CodigoUsuario { get; set; } = string.Empty;

        // Relacionamentos

        [ForeignKey(nameof(CodigoProduto))]
        public Produto Produto { get; set; } = null!;

        [ForeignKey(nameof(CodigoCosif))]
        public Cosif Cosif { get; set; } = null!;
    }
}
