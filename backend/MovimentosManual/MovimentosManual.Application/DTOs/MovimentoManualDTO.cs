namespace MovimentosManual.Application.DTOs
{
    public class MovimentoManualDTO
    {
        public decimal Mes { get; set; }
        public decimal Ano { get; set; }
        public long NumeroLancamento { get; set; }
        public string CodigoProduto { get; set; } = string.Empty;
        public string CodigoCosif { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataMovimento { get; set; }
        public string CodigoUsuario { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
