using System;

namespace MovimentosManual.Application.Models.Request
{
    public class MovimentoManualRequest
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public long NumeroLancamento { get; set; }
        public string CodigoProduto { get; set; } = string.Empty;
        public string CodigoCosif { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataMovimento { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
