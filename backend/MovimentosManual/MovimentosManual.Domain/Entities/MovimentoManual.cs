using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimentosManual.Domain.Entities
{
    [Table("MOVIMENTO_MANUAL", Schema = "dbo")]
    public class MovimentoManual
    {
        [Column("DAT_MES")]
        public decimal Mes { get; set; }  // Corrigido: de int para decimal

        [Column("DAT_ANO")]
        public decimal Ano { get; set; }  // Corrigido: de int para decimal

        [Key]
        [Column("NUM_LANCAMENTO", TypeName = "numeric(18,0)")]
        public long NumeroLancamento { get; set; }

        [Column("COD_PRODUTO")]
        public string CodigoProduto { get; set; } = string.Empty;

        [Column("COD_COSIF")]
        public string CodigoCosif { get; set; } = string.Empty;

        [Column("DES_DESCRICAO")]
        public string Descricao { get; set; } = string.Empty;

        [Column("DAT_MOVIMENTO")]
        public DateTime DataMovimento { get; set; }

        [Column("COD_USUARIO")]
        public string CodigoUsuario { get; set; } = string.Empty;

        [Column("VAL_VALOR", TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        // Propriedade de navegação
        public ProdutoCosif? ProdutoCosif { get; set; }
    }
}
