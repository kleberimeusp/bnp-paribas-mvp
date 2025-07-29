namespace MovimentosManual.Application.Models.Request
{
    public class ProdutoRequest
    {
        public string CodigoProduto { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
