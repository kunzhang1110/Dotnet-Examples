using ApiExamples.Models;

namespace ApiExamples.Repositories
{
    public interface IArticlesRepository
    {
        Task<List<Article>> GetAllArticlesAsync();
        Task<List<Tag>> GetAllTagsAsync();
        Task<ArticleWithTags?> GetArticleWithTagByIdAsync(int id);
        Task<List<ArticleWithTags>> GetAllArticlesWithTagsAsync();
        Task<Article?> CreateArticleAsync(Article article);
        Task<Article?> UpdateArticleAsync(Article article);
        Task<bool?> DeleteByIdArticleAsync(int id);
    }
}
