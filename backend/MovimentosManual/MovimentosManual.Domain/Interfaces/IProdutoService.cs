using MovimentosManual.Core.Common;
using MovimentosManual.Core.Pagination;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Core.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ListarTodos();
        Task<Produto?> ObterPorId(int id);
        Task<Produto?> Obter(string codigo);
        Task Incluir(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(string codigo);
        Task<PagedResult<Produto>> BuscarPaginado(PagedQuery<Produto> query);
        Task<PagedResult<Produto>> BuscarPaginadoComFiltros(string? codigo, string? descricao, int page, int pageSize);
        Task<PagedResult<Produto>> GetPagedAsync(PagedRequestWithSort<Produto> request);
    }
}
