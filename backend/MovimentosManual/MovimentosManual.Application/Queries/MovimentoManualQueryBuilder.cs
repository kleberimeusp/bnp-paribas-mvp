using MovimentosManual.Application.Models.Filter;
using MovimentosManual.Application.Queries;
using MovimentosManual.Domain.Entities;
using MovimentosManual.Infrastructure.Common.Linq;
using System.Linq.Expressions;

public class MovimentoManualQueryBuilder : IMovimentoManualQueryBuilder
{
    public Expression<Func<MovimentoManual, bool>> BuildAdvancedFilter(MovimentoManualFilter? filter)
    {
        throw new NotImplementedException();
    }

    public Expression<Func<MovimentoManual, bool>> BuildBasicFilter(int? ano, int? mes, string? produto)
    {
        var filtro = PredicateBuilder.True<MovimentoManual>();

        if (ano.HasValue)
            filtro = filtro.And(x => x.Ano == ano.Value);

        if (mes.HasValue)
            filtro = filtro.And(x => x.Mes == mes.Value);

        if (!string.IsNullOrWhiteSpace(produto))
            filtro = filtro.And(x => x.CodigoProduto.Contains(produto));

        return filtro;
    }

    public Dictionary<string, Expression<Func<MovimentoManual, object>>> GetOrderingMap()
    {
        throw new NotImplementedException();
    }
}
