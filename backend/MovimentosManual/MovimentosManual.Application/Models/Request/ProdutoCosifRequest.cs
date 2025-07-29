namespace MovimentosManual.Application.Models.Request
{
    public class ProdutoCosifRequest
    {
        public string CodigoProduto { get; set; } = string.Empty;
        public string CodigoCosif { get; set; } = string.Empty;
        public string CodigoClassificacao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
