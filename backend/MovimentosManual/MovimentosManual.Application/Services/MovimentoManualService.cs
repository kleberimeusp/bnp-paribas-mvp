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
    public class MovimentoManualService : IMovimentoManualService
    {
        private readonly IRepository<MovimentoManual> _repository;

        public MovimentoManualService(IRepository<MovimentoManual> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MovimentoManual>> ListarTodos()
        {
            return await _repository.ListarTodos();
        }

        public Task<MovimentoManual?> ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public Task<MovimentoManual?> Obter(int id)
        {
            return _repository.ObterPorId(id);
        }

        public Task<MovimentoManual?> Obter(int mes, int ano, long numeroLancamento)
        {
            return Task.FromResult(
                _repository.Query().FirstOrDefault(x =>
                    x.Mes == mes &&
                    x.Ano == ano &&
                    x.NumeroLancamento == numeroLancamento
                )
            );
        }

        public async Task Incluir(MovimentoManual entidade)
        {
            await _repository.Incluir(entidade);
        }

        public async Task Atualizar(MovimentoManual entidade)
        {
            await _repository.Atualizar(entidade);
        }

        public async Task Remover(int id)
        {
            var entidade = await _repository.ObterPorId(id);
            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public async Task Remover(int mes, int ano, long numeroLancamento)
        {
            var entidade = _repository.Query().FirstOrDefault(x =>
                x.Mes == mes &&
                x.Ano == ano &&
                x.NumeroLancamento == numeroLancamento
            );

            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public async Task Remover(string codigoProduto, string codigoCosif, int mes, int ano)
        {
            var entidade = _repository.Query().FirstOrDefault(x =>
                x.CodigoProduto == codigoProduto &&
                x.CodigoCosif == codigoCosif &&
                x.Mes == mes &&
                x.Ano == ano
            );

            if (entidade != null)
                await _repository.Remover(entidade);
        }

        public Task<MovimentoManual?> Obter(Func<MovimentoManual, bool> predicate)
        {
            return Task.FromResult(_repository.Query().FirstOrDefault(predicate));
        }

        public async Task<PagedResult<MovimentoManual>> BuscarPaginado(PagedQuery<MovimentoManual> query)
        {
            var source = _repository.Query();

            if (query.Filter != null)
                source = source.Where(query.Filter);

            if (query.Filters != null)
                source = query.Filters(source);

            if (query.Orderings?.Any() == true)
            {
                IOrderedQueryable<MovimentoManual>? ordered = null;
                foreach (var order in query.Orderings)
                {
                    if (ordered == null)
                    {
                        ordered = order.Descending
                            ? source.OrderByDescending(order.Expression)
                            : source.OrderBy(order.Expression);
                    }
                    else
                    {
                        ordered = order.Descending
                            ? ordered.ThenByDescending(order.Expression)
                            : ordered.ThenBy(order.Expression);
                    }
                }
                source = ordered ?? source;
            }

            var total = await source.CountAsync();
            var items = await source.Skip((query.Page - 1) * query.PageSize)
                                    .Take(query.PageSize)
                                    .ToListAsync();

            return new PagedResult<MovimentoManual>
            {
                Items = items,
                TotalItems = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<MovimentoManual>> GetPagedAsync(PagedQuery<MovimentoManual> query)
        {
            return await BuscarPaginado(query);
        }

        public async Task<PagedResult<MovimentoManual>> GetPagedAsync(PagedRequestWithSort<MovimentoManualFilter> request)
        {
            var source = _repository.Query();
            var filtro = PredicateBuilder.True<MovimentoManual>();

            if (request.Filters != null)
            {
                if (request.Filters.Ano.HasValue)
                    filtro = filtro.And(m => m.Ano == request.Filters.Ano.Value);

                if (request.Filters.Mes.HasValue)
                    filtro = filtro.And(m => m.Mes == request.Filters.Mes.Value);

                if (!string.IsNullOrWhiteSpace(request.Filters.CodigoProduto))
                    filtro = filtro.And(m => m.CodigoProduto.Contains(request.Filters.CodigoProduto));

                if (!string.IsNullOrWhiteSpace(request.Filters.CodigoCosif))
                    filtro = filtro.And(m => m.CodigoCosif.Contains(request.Filters.CodigoCosif));

                if (!string.IsNullOrWhiteSpace(request.Filters.Descricao))
                    filtro = filtro.And(m => m.Descricao.Contains(request.Filters.Descricao));

                if (request.Filters.ValorMinimo.HasValue)
                    filtro = filtro.And(m => m.Valor >= request.Filters.ValorMinimo.Value);

                if (request.Filters.ValorMaximo.HasValue)
                    filtro = filtro.And(m => m.Valor <= request.Filters.ValorMaximo.Value);
            }

            source = source.Where(filtro);

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                source = request.Descending
                    ? source.OrderByDescending(x => EF.Property<object>(x, request.OrderBy))
                    : source.OrderBy(x => EF.Property<object>(x, request.OrderBy));
            }

            return PagedResult<MovimentoManual>.Create(source, request.Page, request.PageSize);
        }

        public Task<PagedResult<MovimentoManual>> BuscarPaginadoComFiltros(string? codigoProduto, int? ano, int? mes, int page, int pageSize)
        {
            var source = _repository.Query();

            if (!string.IsNullOrWhiteSpace(codigoProduto))
                source = source.Where(x => x.CodigoProduto.Contains(codigoProduto));

            if (ano.HasValue)
                source = source.Where(x => x.Ano == ano);

            if (mes.HasValue)
                source = source.Where(x => x.Mes == mes);

            return Task.FromResult(PagedResult<MovimentoManual>.Create(source, page, pageSize));
        }

        public Task<PagedResult<MovimentoManual>> GetPagedAsync(PagedRequestWithSort<MovimentoManual> request)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MovimentoManual>> BuscarPaginadoComQuery(PagedQuery<MovimentoManual> query)
        {
            throw new NotImplementedException();
        }

    }
}