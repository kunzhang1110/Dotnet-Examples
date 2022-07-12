using EFCoreExamples.Models;

namespace EFCoreExamples.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly EFCoreExamplesContext _context = new EFCoreExamplesContext();
        private GenericRepository<Article>? _articleRepository;
        private GenericRepository<Tag>? _tagRepository;

        public GenericRepository<Article> ArticleRepository
        {
            get
            {
                _articleRepository ??= new GenericRepository<Article>(_context);
                return _articleRepository;
            }
        }

        public GenericRepository<Tag> TagRepository
        {
            get
            {
                _tagRepository ??= new GenericRepository<Tag>(_context);
                return _tagRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false; // To detect redundant calls 
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        //standard implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
