using System;
using System.Linq.Expressions;

namespace MovimentosManual.Infrastructure.Common.Linq;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>() => x => true;
    public static Expression<Func<T, bool>> False<T>() => x => false;

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));
        var left = ReplaceParameter(expr1.Body, expr1.Parameters[0], parameter);
        var right = ReplaceParameter(expr2.Body, expr2.Parameters[0], parameter);
        var body = Expression.OrElse(left, right);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));
        var left = ReplaceParameter(expr1.Body, expr1.Parameters[0], parameter);
        var right = ReplaceParameter(expr2.Body, expr2.Parameters[0], parameter);
        var body = Expression.AndAlso(left, right);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression ReplaceParameter(
        Expression expression,
        ParameterExpression source,
        ParameterExpression target)
    {
        return new ReplaceParameterVisitor(source, target).Visit(expression)!;
    }

    private class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _source;
        private readonly ParameterExpression _target;

        public ReplaceParameterVisitor(ParameterExpression source, ParameterExpression target)
        {
            _source = source;
            _target = target;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _source ? _target : base.VisitParameter(node);
        }
    }
}
