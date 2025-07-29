namespace MovimentosManual.Application.Models.Dto
{
    public class MovimentoManualDto
    {
        public string Mes { get; set; } = default!;
        public string Ano { get; set; } = default!;
        public string CodigoProduto { get; set; } = default!;
        public string CodigoCosif { get; set; } = default!;
        public string Descricao { get; set; } = default!;
        public decimal Valor { get; set; }
    }
}
