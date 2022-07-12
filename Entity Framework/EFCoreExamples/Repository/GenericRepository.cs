using EFCoreExamples.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExamples.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly EFCoreExamplesContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(EFCoreExamplesContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public void DeleteAll()
        {
            _dbSet.RemoveRange(_dbSet);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
