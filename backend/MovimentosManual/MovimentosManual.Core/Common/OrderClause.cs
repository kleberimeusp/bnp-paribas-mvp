using System;
using System.Linq.Expressions;

namespace MovimentosManual.Core.Common
{
    /// <summary>
    /// Representa uma cláusula de ordenação genérica para consultas paginadas.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    public class OrderClause<T>
    {
        /// <summary>
        /// Construtor da cláusula de ordenação.
        /// </summary>
        /// <param name="keySelector">Expressão lambda que define o campo para ordenação.</param>
        /// <param name="descending">Define se a ordenação será descendente.</param>
        public OrderClause(Expression<Func<T, object>> keySelector, bool descending = false)
        {
            KeySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            Descending = descending;
        }

        /// <summary>
        /// Expressão que representa o campo de ordenação.
        /// </summary>
        public Expression<Func<T, object>> KeySelector { get; }

        /// <summary>
        /// Define se a ordenação é descendente.
        /// </summary>
        public bool Descending { get; }

        /// <summary>
        /// Alias alternativo para KeySelector, usado em cenários genéricos.
        /// </summary>
        public Expression<Func<T, object>> Expression => KeySelector;
    }
}
