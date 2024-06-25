using ApiExamples.Controllers;
using ApiExamples.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiExamples.Repositories
{
    public class ArticlesRepository: IArticlesRepository
    {
        private readonly ApiExamplesContext _context;
        private readonly ILogger? _logger;

        public ArticlesRepository(ApiExamplesContext context, ILogger<ArticlesController>? logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }


        public async Task<ArticleWithTags?> GetArticleWithTagByIdAsync(int id)
        {
            var article = await _context.Articles
                      .Where(a => a.Id == id)
                      .Include(a => a.ArticleTags)
                      .ThenInclude(at => at.Tag)
                      .FirstOrDefaultAsync();

            if (article == null)
            {
                return null;
            }

            var articleWithTags = new ArticleWithTags
            {
                Id= article.Id,
                Date = article.Date,
                Title = article.Title,
                Tags = article.ArticleTags.Select(a => a.Tag?.Name).ToList()
            };

            return articleWithTags;
        }

        public async Task<List<ArticleWithTags>> GetAllArticlesWithTagsAsync()
        {
            var articles = await _context.Articles
                 .Include(a => a.ArticleTags)
                 .ThenInclude(at => at.Tag).ToListAsync();

            var articlesWithTags = new List<ArticleWithTags>();
            articles.ForEach(a =>

                articlesWithTags.Add(new ArticleWithTags
                {
                    Id = a.Id,
                    Date = a.Date,
                    Title = a.Title,
                    Tags = a.ArticleTags.Select(at => at.Tag?.Name).ToList()
                })
            );

            return articlesWithTags;
        }

        public async Task<Article?> CreateArticleAsync(Article article)
        {
            try
            {
                _context.Articles.Add(article);
                await _SaveChangesAsync();
                return article;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<Article?> UpdateArticleAsync(Article article)
        {
            try
            {
                _context.Entry(article).State = EntityState.Modified;
                await _SaveChangesAsync();
                return article;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _CheckArticleExists(article.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool?> DeleteByIdArticleAsync(int id)
        {
            try
            {

                var article = await _context.Articles.FindAsync(id);
                if (article == null)
                {
                    return null;
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task _SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        private async Task<bool> _CheckArticleExists(int id)
        {
            return await _context.Articles.AnyAsync(e => e.Id == id);
        }
    }
}
