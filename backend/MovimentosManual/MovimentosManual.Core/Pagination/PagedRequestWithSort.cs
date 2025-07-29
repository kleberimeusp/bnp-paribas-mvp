namespace MovimentosManual.Core.Pagination
{
    /// <summary>
    /// Representa uma requisição de dados paginados com filtro e ordenação.
    /// </summary>
    /// <typeparam name="T">Tipo do DTO de filtro (ex: CosifFilterDto)</typeparam>
    public class PagedRequestWithSort<T>
    {
        /// <summary>
        /// Filtros aplicados à consulta.
        /// </summary>
        public T? Filters { get; set; }

        /// <summary>
        /// Número da página (1 baseado).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Quantidade de itens por página.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Campo pelo qual ordenar (ex: "Descricao", "Status").
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Define se a ordenação é descendente.
        /// </summary>
        public bool Descending { get; set; } = false;
    }
}
