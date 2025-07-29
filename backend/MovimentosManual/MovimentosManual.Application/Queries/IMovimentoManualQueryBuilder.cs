using MovimentosManual.Domain.Entities;
using System.Linq.Expressions;
using MovimentosManual.Application.Models.Filter;

namespace MovimentosManual.Application.Queries
{
    public interface IMovimentoManualQueryBuilder
    {
        /// <summary>
        /// Constrói expressão de filtro básica (ano, mês, produto).
        /// </summary>
        Expression<Func<MovimentoManual, bool>> BuildBasicFilter(int? ano, int? mes, string? produto);

        /// <summary>
        /// Constrói expressão de filtro com base em DTO de filtro.
        /// </summary>
        Expression<Func<MovimentoManual, bool>> BuildAdvancedFilter(MovimentoManualFilter? filter);

        /// <summary>
        /// Mapeia campo de ordenação para expressão de propriedade.
        /// </summary>
        Dictionary<string, Expression<Func<MovimentoManual, object>>> GetOrderingMap();
    }
}
