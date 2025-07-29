namespace MovimentosManual.Application.Models.Dto
{
    public class ProdutoCosifDto
    {
        public string CodigoProduto { get; set; } = default!;
        public string CodigoCosif { get; set; } = default!;
        public string? CodigoClassificacao { get; set; }
        public string? Status { get; set; }
    }
}
