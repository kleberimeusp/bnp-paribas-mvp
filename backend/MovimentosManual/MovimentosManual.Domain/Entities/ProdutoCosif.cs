using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimentosManual.Domain.Entities
{
    [Table("PRODUTO_COSIF", Schema = "dbo")]
    public class ProdutoCosif
    {
        [Column("COD_PRODUTO")]
        [StringLength(20)] // ajuste conforme tamanho real na base
        public string CodigoProduto { get; set; } = string.Empty;

        [Column("COD_COSIF")]
        [StringLength(20)]
        public string CodigoCosif { get; set; } = string.Empty;

        [Column("COD_CLASSIFICACAO")]
        [StringLength(20)]
        public string CodigoClassificacao { get; set; } = string.Empty;

        [Column("STA_STATUS")]
        [StringLength(1)]
        public string Status { get; set; } = string.Empty;

        // Relacionamento com Produto (1:1 ou N:1)
        public Produto? Produto { get; set; }

        // Relacionamento com MovimentoManual (1:N)
        public ICollection<MovimentoManual> Movimentos { get; set; } = new List<MovimentoManual>();
    }
}
