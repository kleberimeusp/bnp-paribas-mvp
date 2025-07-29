using Microsoft.EntityFrameworkCore;
using MovimentosManual.Domain.Interfaces;
using MovimentosManual.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovimentosManual.Infrastructure.Services
{
    public class CompositeRepository<TEntity> : ICompositeRepository<TEntity>
        where TEntity : class
    {
        protected readonly MovimentosDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public CompositeRepository(MovimentosDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> ListarTodos()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity?> ObterPorIds(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }

        public virtual async Task Incluir(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Remover(params object[] keys)
        {
            var entity = await ObterPorIds(keys);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
