using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovimentosManual.Domain.Entities
{
    public enum StatusCosif
    {
        Ativo = 'A',
        Inativo = 'I'
    }

    [Table("COSIF", Schema = "dbo")]
    public class Cosif
    {
        [Key]
        [Column("COD_COSIF")]
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string CodigoCosif { get; set; } = string.Empty;

        [Column("DES_COSIF")]
        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Column("STA_STATUS")]
        [Required]
        [StringLength(1)]
        [RegularExpression("[AI]", ErrorMessage = "Status deve ser 'A' ou 'I'")]
        public string Status { get; set; } = "A";

        // Propriedade auxiliar para uso seguro com Enum
        [NotMapped]
        public StatusCosif StatusEnum
        {
            get => Status == "A" ? StatusCosif.Ativo : StatusCosif.Inativo;
            set => Status = value == StatusCosif.Ativo ? "A" : "I";
        }

        // Relacionamentos
        public ICollection<ProdutoCosif> ProdutosCosif { get; set; } = new List<ProdutoCosif>();
        public ICollection<MovimentoManual> Movimentos { get; set; } = new List<MovimentoManual>();

    }
}
