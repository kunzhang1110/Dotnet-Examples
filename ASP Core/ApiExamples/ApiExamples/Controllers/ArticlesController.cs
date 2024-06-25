using Microsoft.AspNetCore.Mvc;
using ApiExamples.Models;
using ApiExamples.Repositories;


namespace ApiExamples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticlesRepository _repo;
        private readonly ILogger _logger;

        public ArticlesController(ArticlesRepository repo, ILogger<ArticlesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _repo.GetAllArticlesAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleWithTags>> GetArticle(int id)
        {

            var articleWithTag = await _repo.GetArticleWithTagByIdAsync(id);

            if (articleWithTag == null)
            {
                return NotFound();
            }

            return articleWithTag;
        }


        [HttpPut("{id}")]
        // PUT: api/Articles/1003
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            var result = await _repo.UpdateArticleAsync(article);
            if (result == null) return NotFound();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            var result = await _repo.CreateArticleAsync(article);
            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);

        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var result = await _repo.DeleteByIdArticleAsync(id);
            if (result == null) return NotFound();
            return NoContent();
        }


    }
}
