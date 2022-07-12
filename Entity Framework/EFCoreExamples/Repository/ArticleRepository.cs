using EFCoreExamples.Models;


namespace EFCoreExamples.Repository
{
    public class ArticleRepository
    {
        private readonly EFCoreExamplesContext _context;

        public ArticleRepository(EFCoreExamplesContext context)
        {
            _context = context;
        }

        public Article GetFirstByTitle(string title) => _context.Articles.Where(a => a.Title == title).First();
        public List<Article> GetAll() => _context.Articles.ToList();
        public void Delete(Article article) => _context.Articles.Remove(article);
        public void DeleteAll() => _context.Articles.RemoveRange(_context.Articles);
        public void Add(Article article) => _context.Articles.Add(article);
        public void SaveChanges() => _context.SaveChanges();

    }
}
