using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Core.Common;
using MovimentosManual.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MovimentosManual.Infrastructure.Common.Linq;
using MovimentosManual.Application.Models.Filter;

namespace MovimentosManual.Application.Services
{
    public class ProdutoCosifService : IProdutoCosifService
    {
        private readonly IRepository<ProdutoCosif> _repository;

        public ProdutoCosifService(IRepository<ProdutoCosif> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProdutoCosif>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public async Task<ProdutoCosif?> ObterPorId(int id)
        {
            return await _repository.ObterPorId(id);
        }

        public Task<ProdutoCosif?> Obter(Func<ProdutoCosif, bool> predicate)
        {
            return Task.FromResult(_repository.Query().FirstOrDefault(predicate));
        }

        public async Task Incluir(ProdutoCosif entidade)
        {
            await _repository.Incluir(entidade);
        }

        public async Task Atualizar(ProdutoCosif entidade)
        {
            await _repository.Atualizar(entidade);
        }

        public async Task Remover(int id)
        {
            var entidade = await _repository.ObterPorId(id);
            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public async Task<ProdutoCosif?> ObterPorChave(string codigoProduto, string codigoCosif)
        {
            return await _repository.Query()
                                     .FirstOrDefaultAsync(pc => pc.CodigoProduto == codigoProduto && pc.CodigoCosif == codigoCosif);
        }

        public async Task Remover(string codigoProduto, string codigoCosif)
        {
            var entidade = await ObterPorChave(codigoProduto, codigoCosif);
            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public async Task<PagedResult<ProdutoCosif>> BuscarPaginado(PagedQuery<ProdutoCosif> query)
        {
            var baseQuery = _repository.Query();

            if (query.Filter != null)
                baseQuery = baseQuery.Where(query.Filter);

            if (query.Filters != null)
                baseQuery = query.Filters(baseQuery);

            if (query.Orderings != null && query.Orderings.Any())
            {
                IOrderedQueryable<ProdutoCosif>? orderedQuery = null;
                for (int i = 0; i < query.Orderings.Count; i++)
                {
                    var order = query.Orderings[i];
                    if (i == 0)
                    {
                        orderedQuery = order.Descending
                            ? baseQuery.OrderByDescending(order.Expression)
                            : baseQuery.OrderBy(order.Expression);
                    }
                    else if (orderedQuery != null)
                    {
                        orderedQuery = order.Descending
                            ? orderedQuery.ThenByDescending(order.Expression)
                            : orderedQuery.ThenBy(order.Expression);
                    }
                }
                baseQuery = orderedQuery ?? baseQuery;
            }

            var total = await baseQuery.CountAsync();
            var items = await baseQuery.Skip((query.Page - 1) * query.PageSize)
                                       .Take(query.PageSize)
                                       .ToListAsync();

            return new PagedResult<ProdutoCosif>
            {
                Items = items,
                TotalItems = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<ProdutoCosif>> GetPagedAsync(PagedQuery<ProdutoCosif> query)
        {
            return await BuscarPaginado(query);
        }

        public async Task<PagedResult<ProdutoCosif>> GetPagedAsync(PagedRequestWithSort<ProdutoCosifFilter> request)
        {
            var source = _repository.Query();
            var filtro = PredicateBuilder.True<ProdutoCosif>();

            if (request.Filters != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Filters.CodigoProduto))
                    filtro = filtro.And(c => c.CodigoProduto.Contains(request.Filters.CodigoProduto));

                if (!string.IsNullOrWhiteSpace(request.Filters.CodigoCosif))
                    filtro = filtro.And(c => c.CodigoCosif.Contains(request.Filters.CodigoCosif));

                if (!string.IsNullOrWhiteSpace(request.Filters.Status))
                    filtro = filtro.And(c => c.Status == request.Filters.Status);
            }

            source = source.Where(filtro);

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                source = request.Descending
                    ? source.OrderByDescending(x => EF.Property<object>(x, request.OrderBy))
                    : source.OrderBy(x => EF.Property<object>(x, request.OrderBy));
            }

            return PagedResult<ProdutoCosif>.Create(source, request.Page, request.PageSize);
        }

        public Task<ProdutoCosif?> Obter(string produtoId, string cosifId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ProdutoCosif>> GetPagedAsync(PagedRequestWithSort<ProdutoCosif> request)
        {
            throw new NotImplementedException();
        }

    }
}