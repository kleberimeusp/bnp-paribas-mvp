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
        [StringLength(20)]
        public string CodigoProduto { get; set; } = string.Empty;

        [Column("DES_PRODUTO")]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Column("STA_STATUS")]
        [StringLength(1)]
        public string Status { get; set; } = string.Empty;

        public ICollection<ProdutoCosif> ProdutosCosif { get; set; } = new List<ProdutoCosif>();
        public ICollection<MovimentoManual> Movimentos { get; set; } = new List<MovimentoManual>();
    }
}
