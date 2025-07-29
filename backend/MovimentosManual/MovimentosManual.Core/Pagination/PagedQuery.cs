using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MovimentosManual.Core.Common;

namespace MovimentosManual.Core.Pagination
{
    /// <summary>
    /// Representa uma consulta paginada com filtros e múltiplas ordenações.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    public class PagedQuery<T>
    {
        /// <summary>
        /// Número da página (base 1).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Quantidade de itens por página.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Lista de cláusulas de ordenação (ordem de prioridade na aplicação da ordenação).
        /// </summary>
        public List<OrderClause<T>> Orderings { get; set; } = new();

        /// <summary>
        /// Expressão de filtro lambda para restringir os resultados.
        /// </summary>
        public Expression<Func<T, bool>>? Filter { get; set; }

        /// <summary>
        /// Filtro funcional adicional que permite composição LINQ avançada.
        /// </summary>
        public Func<IQueryable<T>, IQueryable<T>>? Filters { get; set; }
    }
}
