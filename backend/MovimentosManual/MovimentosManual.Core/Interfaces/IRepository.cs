namespace MovimentosManual.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ListarTodos();
        Task<T?> ObterPorId(int id);
        Task Incluir(T entidade);
        Task Atualizar(T entidade);
        Task Remover(T entidade);
        IQueryable<T> Query();
        
    }
}
