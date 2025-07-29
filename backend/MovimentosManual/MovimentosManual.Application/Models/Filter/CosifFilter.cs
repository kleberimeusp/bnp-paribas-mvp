using System.ComponentModel.DataAnnotations;

namespace MovimentosManual.Application.Models.Filter
{
    /// <summary>
    /// DTO de filtro para consulta paginada de registros COSIF.
    /// </summary>
    public class CosifFilter
    {
        /// <summary>
        /// Código do COSIF (busca exata).
        /// </summary>
        [MaxLength(20, ErrorMessage = "O código COSIF não pode exceder 20 caracteres.")]
        public string? CodigoCosif { get; set; }

        /// <summary>
        /// Descrição (busca por aproximação).
        /// </summary>
        [MaxLength(100, ErrorMessage = "A descrição não pode exceder 100 caracteres.")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Tipo do COSIF (busca exata).
        /// </summary>
        [MaxLength(10, ErrorMessage = "O tipo não pode exceder 10 caracteres.")]
        public string? Tipo { get; set; }

        /// <summary>
        /// Status do COSIF (exemplo: 'Ativo', 'Inativo').
        /// </summary>
        [MaxLength(10, ErrorMessage = "O status não pode exceder 10 caracteres.")]
        public string? Status { get; set; }
    }
}
