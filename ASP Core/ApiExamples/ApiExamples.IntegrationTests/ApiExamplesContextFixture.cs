using ApiExamples.Models;
using ApiExamples.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;


namespace ApiExamples.IntegrationTests
{
    public class ApiExamplesContextFixture : IDisposable
    {
        private readonly ApiExamplesWebApplicationFactory _factory;
        public ApiExamplesContext Context { get; private set; }
        public HttpClient Client { get; private set; }

        public ApiExamplesContextFixture()
        {
            _factory = new ApiExamplesWebApplicationFactory();
            var scope = _factory.Services.CreateScope();
            Client = _factory.CreateClient();
            var scopedServices = scope.ServiceProvider;
            Context = scopedServices.GetRequiredService<ApiExamplesContext>();
            Context.Database.EnsureCreated();
            InitializeDbForTests();
        }

        public void Reset()
        {
           
        }


        public void Dispose()
        {

        }


        public void InitializeDbForTests()
        {
            Context.Articles.AddRange(DbInitializer.SeedArticles);
            Context.Tags.AddRange(DbInitializer.SeedTags);
            Context.SaveChanges();
            var articleTags = new List<ArticleTag?>
            {
                DbInitializer.CreateArticleTag("NoSQL Review","Database",Context),
                DbInitializer.CreateArticleTag("NoSQL Review","MongoDb",Context),
                DbInitializer.CreateArticleTag("Python Review","Python",Context),
                DbInitializer.CreateArticleTag("Financial Analysis","MongoDb",Context),
                DbInitializer.CreateArticleTag("Financial Analysis","Python",Context),
                DbInitializer.CreateArticleTag("Financial Analysis","Finance",Context),

            };
            foreach (var at in articleTags)
            {
                if (at != null) Context.ArticleTags.Add(at);

            }
            Context.SaveChanges();
        }

        public void EraseDbForTests()
        {
            Context.Articles.RemoveRange(Context.Articles);
            Context.SaveChanges();

            Context.Tags.RemoveRange(Context.Tags);
            Context.SaveChanges();

            Context.ArticleTags.RemoveRange(Context.ArticleTags);
            Context.SaveChanges();

        }

    }

}
