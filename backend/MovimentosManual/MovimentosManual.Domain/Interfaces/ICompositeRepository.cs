using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovimentosManual.Domain.Interfaces
{
    public interface ICompositeRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> ListarTodos();
        Task<TEntity?> ObterPorIds(params object[] keys);
        Task Incluir(TEntity entity);
        Task Atualizar(TEntity entity);
        Task Remover(params object[] keys);
    }
}
