using ApiExamples.Data;
using ApiExamples.Models;
using ApiExamples.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ApiExamples.IntegrationTests
{

    public class ArticlesFixture : IDisposable
    {
        private readonly ArticlesWebApplicationFactory _factory;
        public ApiExamplesContext DbContext { get; private set; }
        public HttpClient Client { get; private set; }

        public ArticlesFixture()
        {
            //setup test server and client
            _factory = new ArticlesWebApplicationFactory();
            Client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });
            //setup test database
            using var scope = _factory.Services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<ApiExamplesContext>();
            DbContext.Database.EnsureCreated();//ensure db is created and tables are created.
            InitializeDb();
        }

        public void Dispose()
        {
            EraseDb();
        }

        public void InitializeDb()
        {
            DbContext.Articles.AddRange(DbInitializer.SeedArticles);
            DbContext.Tags.AddRange(DbInitializer.SeedTags);
            DbContext.SaveChanges();
            var articleTags = new List<ArticleTag?>
            {
                DbInitializer.CreateArticleTag("NoSQL Review","Database",DbContext),
                DbInitializer.CreateArticleTag("NoSQL Review","MongoDb",DbContext),
                DbInitializer.CreateArticleTag("Python Review","Python",DbContext),
                DbInitializer.CreateArticleTag("Financial Analysis","MongoDb",DbContext),
                DbInitializer.CreateArticleTag("Financial Analysis","Python",DbContext),
                DbInitializer.CreateArticleTag("Financial Analysis","Finance",DbContext),

            };
            foreach (var at in articleTags)
            {
                if (at != null) DbContext.ArticleTags.Add(at);

            }
            DbContext.SaveChanges();
        }

        public void EraseDb()
        {
            DbContext.Articles.RemoveRange(DbContext.Articles);
            DbContext.SaveChanges();

            DbContext.Tags.RemoveRange(DbContext.Tags);
            DbContext.SaveChanges();

            DbContext.ArticleTags.RemoveRange(DbContext.ArticleTags);
            DbContext.SaveChanges();

        }


    }

}
