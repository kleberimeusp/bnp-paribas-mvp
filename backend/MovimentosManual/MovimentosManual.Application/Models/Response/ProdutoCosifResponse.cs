namespace MovimentosManual.Application.Models.Response
{
    public class ProdutoCosifResponse
    {
        public string CodigoProduto { get; set; } = string.Empty;
        public string CodigoCosif { get; set; } = string.Empty;
        public string CodigoClassificacao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public ProdutoResponse Produto { get; set; } = new();
        public CosifResponse? Cosif { get; set; } // Se existir um Cosif relacionado
    }
}
