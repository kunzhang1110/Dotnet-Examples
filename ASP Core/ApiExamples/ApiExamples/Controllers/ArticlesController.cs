using Microsoft.AspNetCore.Mvc;
using ApiExamples.Models;
using ApiExamples.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiExamples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesRepository _repo;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticlesRepository repo, ILogger<ArticlesController> logger)
        {
            _repo = repo;
            _logger = logger;

        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            var result = await _repo.GetAllArticlesAsync();
            return Ok(result);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<Article>>> GetAdminArticles()
        {
            var userId = HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _repo.GetAllArticlesAsync();
            return Ok(result);
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

            return Ok(articleWithTag);
        }

        // Post: api/Articles
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            var result = await _repo.CreateArticleAsync(article);
            if (result == null) return BadRequest();
            return CreatedAtAction(nameof(GetArticle), new { id = result.Id }, result);

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
            return Ok(result);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var result = await _repo.DeleteByIdArticleAsync(id);
            if (result == null) return NotFound();
            return Ok();
        }

    }
}
