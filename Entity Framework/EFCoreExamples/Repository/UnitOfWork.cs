using EFCoreExamples.Models;

namespace EFCoreExamples.Repository
{
    public class UnitOfWork : IDisposable
    {

        private readonly EFCoreExamplesContext _context;
        private GenericRepository<Article>? _articleRepository;
        private GenericRepository<Tag>? _tagRepository;

        public UnitOfWork(EFCoreExamplesContext context)
        {
            _context = context;
        }

        public GenericRepository<Article> ArticleRepository => _articleRepository ??= new GenericRepository<Article>(_context);

        public GenericRepository<Tag> TagRepository => _tagRepository ??= new GenericRepository<Tag>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
