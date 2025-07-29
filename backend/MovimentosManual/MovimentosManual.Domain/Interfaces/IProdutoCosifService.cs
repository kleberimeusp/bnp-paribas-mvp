using MovimentosManual.Core.Common;
using MovimentosManual.Core.Pagination;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Core.Interfaces
{
    public interface IProdutoCosifService
    {
        /// <summary>
        /// Lista todos os ProdutoCosif.
        /// </summary>
        Task<IEnumerable<ProdutoCosif>> ListarTodos();

        /// <summary>
        /// Obtém um ProdutoCosif por chave composta.
        /// </summary>
        Task<ProdutoCosif?> Obter(string produtoId, string cosifId);

        /// <summary>
        /// Inclui um novo ProdutoCosif.
        /// </summary>
        Task Incluir(ProdutoCosif entidade);

        /// <summary>
        /// Atualiza um ProdutoCosif existente.
        /// </summary>
        Task Atualizar(ProdutoCosif entidade);

        /// <summary>
        /// Remove um ProdutoCosif por chave composta.
        /// </summary>
        Task Remover(string produtoId, string cosifId);

        /// <summary>
        /// Busca paginada com filtros simples via query.
        /// </summary>
        Task<PagedResult<ProdutoCosif>> BuscarPaginado(PagedQuery<ProdutoCosif> query);

        /// <summary>
        /// Busca paginada com ordenação e filtros via DTO.
        /// </summary>
        Task<PagedResult<ProdutoCosif>> GetPagedAsync(PagedRequestWithSort<ProdutoCosif> request);
    }
}
