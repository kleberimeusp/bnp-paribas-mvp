using MovimentosManual.Domain.Entities;
using MovimentosManual.Core.Interfaces;
using MovimentosManual.Core.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MovimentosManual.Core.Pagination;
using MovimentosManual.Infrastructure.Common.Linq;
using MovimentosManual.Application.Models.Filter;

namespace MovimentosManual.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IRepository<Produto> _repository;

        public ProdutoService(IRepository<Produto> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Produto>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public async Task<Produto?> ObterPorId(int id)
        {
            return await _repository.ObterPorId(id);
        }

        public async Task<Produto?> Obter(string codigo)
        {
            return await _repository.Query()
                                    .FirstOrDefaultAsync(p => p.CodigoProduto == codigo);
        }

        public async Task Incluir(Produto entidade)
        {
            await _repository.Incluir(entidade);
        }

        public async Task Atualizar(Produto entidade)
        {
            await _repository.Atualizar(entidade);
        }

    

        public async Task Remover(string codigo)
        {
            var entidade = await Obter(codigo);
            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public async Task<PagedResult<Produto>> BuscarPaginado(PagedQuery<Produto> query)
        {
            var baseQuery = _repository.Query();

            if (query.Filter != null)
                baseQuery = baseQuery.Where(query.Filter);

            if (query.Filters != null)
                baseQuery = query.Filters(baseQuery);

            if (query.Orderings != null && query.Orderings.Any())
            {
                IOrderedQueryable<Produto>? orderedQuery = null;

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

            return new PagedResult<Produto>
            {
                Items = items,
                TotalItems = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<Produto>> GetPagedAsync(PagedQuery<Produto> query)
        {
            return await BuscarPaginado(query);
        }

        public async Task<PagedResult<Produto>> GetPagedAsync(PagedRequestWithSort<ProdutoFilter> request)
        {
            var source = _repository.Query();
            var filtro = PredicateBuilder.True<Produto>();

            if (request.Filters != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Filters.Descricao))
                    filtro = filtro.And(p => p.Descricao.Contains(request.Filters.Descricao));

                if (!string.IsNullOrWhiteSpace(request.Filters.Status))
                    filtro = filtro.And(p => p.Status == request.Filters.Status);
            }

            source = source.Where(filtro);

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                source = request.Descending
                    ? source.OrderByDescending(x => EF.Property<object>(x, request.OrderBy))
                    : source.OrderBy(x => EF.Property<object>(x, request.OrderBy));
            }

            return PagedResult<Produto>.Create(source, request.Page, request.PageSize);
        }

        public async Task<PagedResult<Produto>> BuscarPaginadoComFiltros(string? codigo, string? descricao, int page, int pageSize)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(codigo))
                query = query.Where(p => p.CodigoProduto.Contains(codigo));

            if (!string.IsNullOrWhiteSpace(descricao))
                query = query.Where(p => p.Descricao.Contains(descricao));

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<Produto>
            {
                Items = items,
                TotalItems = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public Task<PagedResult<Produto>> GetPagedAsync(PagedRequestWithSort<Produto> request)
        {
            throw new NotImplementedException();
        }

    }
}
