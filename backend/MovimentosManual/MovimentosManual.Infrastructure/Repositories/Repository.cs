using MovimentosManual.Core.Interfaces;
using MovimentosManual.Infrastructure.Context; // ⬅️ Importação correta do MovimentosDbContext
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovimentosManual.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MovimentosDbContext _context; // ⬅️ Substituído DbContext por MovimentosDbContext
        private readonly DbSet<T> _dbSet;

        public Repository(MovimentosDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> ListarTodos() => await _dbSet.ToListAsync();

        public async Task<T?> ObterPorId(int id) => await _dbSet.FindAsync(id);

        public async Task Incluir(T entidade)
        {
            await _dbSet.AddAsync(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(T entidade)
        {
            _dbSet.Update(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(T entidade)
        {
            _dbSet.Remove(entidade);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Query() => _dbSet.AsQueryable();
    }
}
