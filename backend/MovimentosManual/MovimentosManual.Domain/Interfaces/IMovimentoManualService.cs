using MovimentosManual.Core.Common;
using MovimentosManual.Core.Pagination;
using MovimentosManual.Domain.Entities;

namespace MovimentosManual.Core.Interfaces
{
    public interface IMovimentoManualService
    {
        /// <summary>
        /// Lista todos os lançamentos manuais.
        /// </summary>
        Task<IEnumerable<MovimentoManual>> ListarTodos();

        /// <summary>
        /// Obtém um lançamento manual por chave composta (Mês, Ano, Número).
        /// </summary>
        Task<MovimentoManual?> Obter(int mes, int ano, long numeroLancamento);

        /// <summary>
        /// Cria um novo lançamento manual.
        /// </summary>
        Task Incluir(MovimentoManual entidade);

        /// <summary>
        /// Atualiza um lançamento manual existente.
        /// </summary>
        Task Atualizar(MovimentoManual entidade);

        /// <summary>
        /// Remove por chave composta (Mês, Ano, Número).
        /// </summary>
        Task Remover(int mes, int ano, long numeroLancamento);

        /// <summary>
        /// Remove um lançamento baseado em Produto, Cosif, Mês e Ano.
        /// </summary>
        Task Remover(string codigoProduto, string codigoCosif, int mes, int ano);

        /// <summary>
        /// Busca paginada com filtro direto por expressão.
        /// </summary>
        Task<PagedResult<MovimentoManual>> BuscarPaginado(PagedQuery<MovimentoManual> query);

        /// <summary>
        /// Busca paginada com filtro e ordenação baseada em DTO.
        /// </summary>
        Task<PagedResult<MovimentoManual>> GetPagedAsync(PagedRequestWithSort<MovimentoManual> request);

        /// <summary>
        /// Busca paginada com filtros dinâmicos via query (Expression<Func>).
        /// </summary>
        Task<PagedResult<MovimentoManual>> BuscarPaginadoComQuery(PagedQuery<MovimentoManual> query);

        Task<PagedResult<MovimentoManual>> BuscarPaginadoComFiltros(
            string? codigoProduto, int? ano, int? mes, int page, int pageSize);

            

    }
}
