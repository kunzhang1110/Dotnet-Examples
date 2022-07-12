using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiExample.Models;


namespace ApiExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ApiExampleContext _context;
        private readonly ILogger _logger;

        public ArticlesController(ApiExampleContext context, ILogger<ArticlesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleWithTags>> GetArticle(int id)
        {

            var article = await _context.Articles
                .Where(a => a.Id == id)
                .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
                .FirstOrDefaultAsync();

            if (article == null)
            {
                return NotFound();
            }

            var articleWithTags = new ArticleWithTags
            {
                Date = article.Date,
                Title = article.Title,
                Tags = article.ArticleTags.Select(a => a.Tag.Name).ToList()
            };

            return articleWithTags;
        }


        [HttpPut("{id}")]
        // PUT: api/Articles/1003
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return (_context.Articles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
