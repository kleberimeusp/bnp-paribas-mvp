namespace MovimentosManual.Application.Models.Response
{

    public class MovimentoManualResponse
    {
        private ProdutoCosifResponse produtoCosif = new();

        public int Mes { get; set; }
        public int Ano { get; set; }
        public long NumeroLancamento { get; set; }
        public string CodigoProduto { get; set; } = string.Empty;
        public string CodigoCosif { get; set; } = string.Empty;
        public DateTime DataMovimento { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }


        public ProdutoCosifResponse ProdutoCosif { get => produtoCosif; set => produtoCosif = value; }
    }

}