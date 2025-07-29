using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimentosManual.Domain.Entities
{
    [Table("PRODUTO_COSIF", Schema = "dbo")]
    public class ProdutoCosif
    {
        [Column("COD_PRODUTO")]
        [StringLength(20)]
        public string CodigoProduto { get; set; } = string.Empty;

        [Column("COD_COSIF")]
        [StringLength(20)]
        public string CodigoCosif { get; set; } = string.Empty;

        [Column("COD_CLASSIFICACAO")]
        [StringLength(10)]
        public string CodigoClassificacao { get; set; } = string.Empty;

        [Column("STA_STATUS")]
        [StringLength(1)]
        public string Status { get; set; } = string.Empty;

        [ForeignKey(nameof(CodigoProduto))]
        public Produto Produto { get; set; } = null!;

        [ForeignKey(nameof(CodigoCosif))]
        public Cosif Cosif { get; set; } = null!;
    }
}
