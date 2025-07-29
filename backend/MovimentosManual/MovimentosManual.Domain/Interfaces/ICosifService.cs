using MovimentosManual.Core.Common;
using MovimentosManual.Core.Pagination;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Core.Interfaces
{
    public interface ICosifService
    {
        Task<IEnumerable<Cosif>> ListarTodos();
        Task<Cosif?> ObterPorId(string id);
        Task<Cosif?> ObterPorCodigo(string codigo);
        Task Incluir(Cosif entidade);
        Task Atualizar(Cosif entidade);
        Task Remover(string id);
        Task<PagedResult<Cosif>> BuscarPaginado(PagedQuery<Cosif> query);
        Task<PagedResult<Cosif>> GetPagedAsync(PagedQuery<Cosif> query);
    }
}
