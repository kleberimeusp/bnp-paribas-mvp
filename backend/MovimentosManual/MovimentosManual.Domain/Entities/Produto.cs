using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimentosManual.Domain.Entities
{
    [Table("PRODUTO", Schema = "dbo")]
    public class Produto
    {
        [Key]
        [Column("COD_PRODUTO")]
        [StringLength(20)] // ajustar conforme tamanho real no banco
        public string CodigoProduto { get; set; } = string.Empty;

        [Column("DES_PRODUTO")]
        [StringLength(100)] // ajustar conforme tamanho real no banco
        public string Descricao { get; set; } = string.Empty;

        [Column("STA_STATUS")]
        [StringLength(1)] // geralmente 'A' ou 'I'
        public string Status { get; set; } = string.Empty;

        // Relacionamento com ProdutoCosif (1:N)
        public ICollection<ProdutoCosif> ProdutosCosif { get; set; } = new List<ProdutoCosif>();
    }
}
