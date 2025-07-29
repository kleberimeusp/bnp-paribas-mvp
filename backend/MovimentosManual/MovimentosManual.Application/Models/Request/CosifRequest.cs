namespace MovimentosManual.Application.Models.Request
{
    public class CosifRequest
    {
        public string CodigoCosif { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = "A";
    }
}