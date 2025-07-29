using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Core.Common;
using MovimentosManual.Core.Pagination;
using Microsoft.EntityFrameworkCore;

namespace MovimentosManual.Application.Services
{
    public class CosifService : ICosifService
    {
        private readonly IRepository<Cosif> _repository;

        public CosifService(IRepository<Cosif> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Cosif>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public async Task<Cosif?> ObterPorId(int id)
        {
            return await _repository.ObterPorId(id);
        }

        public async Task<Cosif?> Obter(Func<Cosif, bool> predicate)
        {
            return _repository.Query().FirstOrDefault(predicate);
        }

        public async Task Incluir(Cosif entidade)
        {
            await _repository.Incluir(entidade);
        }

        public async Task Atualizar(Cosif entidade)
        {
            await _repository.Atualizar(entidade);
        }

        public async Task Remover(int id)
        {
            var entidade = await _repository.ObterPorId(id);
            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public async Task<Cosif?> ObterPorId(string id)
        {
            return await _repository.Query()
                                     .FirstOrDefaultAsync(c => c.CodigoCosif == id);
        }

        public async Task<Cosif?> ObterPorCodigo(string codigo)
        {
            return await _repository.Query()
                .FirstOrDefaultAsync(c => c.CodigoCosif == codigo);
        }



        public async Task Remover(string id)
        {
            var entidade = await ObterPorCodigo(id);
            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public async Task<PagedResult<Cosif>> BuscarPaginado(PagedQuery<Cosif> query)
        {
            var baseQuery = _repository.Query();

            if (query.Filter != null)
                baseQuery = baseQuery.Where(query.Filter);

            if (query.Filters != null)
                baseQuery = query.Filters(baseQuery);

            if (query.Orderings != null && query.Orderings.Any())
            {
                IOrderedQueryable<Cosif>? orderedQuery = null;
                for (int i = 0; i < query.Orderings.Count; i++)
                {
                    var order = query.Orderings[i];
                    if (i == 0)
                    {
                        orderedQuery = order.Descending
                            ? baseQuery.OrderByDescending(order.KeySelector)
                            : baseQuery.OrderBy(order.KeySelector);
                    }
                    else if (orderedQuery != null)
                    {
                        orderedQuery = order.Descending
                            ? orderedQuery.ThenByDescending(order.KeySelector)
                            : orderedQuery.ThenBy(order.KeySelector);
                    }
                }
                baseQuery = orderedQuery ?? baseQuery;
            }

            var total = await baseQuery.CountAsync();
            var items = await baseQuery.Skip((query.Page - 1) * query.PageSize)
                                        .Take(query.PageSize)
                                        .ToListAsync();

            return new PagedResult<Cosif>
            {
                Items = items,
                TotalItems = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<Cosif>> GetPagedAsync(PagedQuery<Cosif> query)
        {
            return await BuscarPaginado(query);
        }
    }
}
