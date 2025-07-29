using System.ComponentModel.DataAnnotations;

namespace MovimentosManual.Application.Models.Filter
{
    /// <summary>
    /// DTO de filtro para busca paginada de lançamentos manuais.
    /// </summary>
    public class MovimentoManualFilter
    {
        /// <summary>
        /// Ano do lançamento (exato).
        /// </summary>
        [Range(1900, 2100, ErrorMessage = "Ano inválido.")]
        public int? Ano { get; set; }

        /// <summary>
        /// Mês do lançamento (exato, entre 1 e 12).
        /// </summary>
        [Range(1, 12, ErrorMessage = "Mês deve estar entre 1 e 12.")]
        public int? Mes { get; set; }

        /// <summary>
        /// Código do produto (parcial).
        /// </summary>
        [MaxLength(20, ErrorMessage = "Código do produto não pode exceder 20 caracteres.")]
        public string? CodigoProduto { get; set; }

        /// <summary>
        /// Código do COSIF (parcial).
        /// </summary>
        [MaxLength(20, ErrorMessage = "Código do COSIF não pode exceder 20 caracteres.")]
        public string? CodigoCosif { get; set; }

        /// <summary>
        /// Descrição do lançamento (parcial).
        /// </summary>
        [MaxLength(100, ErrorMessage = "Descrição não pode exceder 100 caracteres.")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Valor mínimo do lançamento.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Valor mínimo deve ser maior ou igual a zero.")]
        public decimal? ValorMinimo { get; set; }

        /// <summary>
        /// Valor máximo do lançamento.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Valor máximo deve ser maior ou igual a zero.")]
        public decimal? ValorMaximo { get; set; }
    }
}
