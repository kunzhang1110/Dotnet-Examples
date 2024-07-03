using ApiExamples.DTOs;
using ApiExamples.Models;
using ApiExamples.Utils;

using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiExamples.IntegrationTests
{
    [CollectionDefinition("SequentialTests", DisableParallelization = true)]
    public class ArticlesTestsCollectionDefinition : ICollectionFixture<ArticlesFixture> { }

    [Collection("SequentialTests")]
    public class ArticlesGetTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly ArticlesFixture _fixture;
        private readonly HttpClient _client;

        public ArticlesGetTests(ITestOutputHelper logger, ArticlesFixture fixture)
        {
            _logger = logger;
            _fixture = fixture;
            _client = _fixture.Client;
        }

        [Fact]
        public async Task GetArticles_Always_ReturnsAllArticles()
        {
            var response = await _client.GetAsync("api/Articles");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = JsonConvert.DeserializeObject<IEnumerable<Article>>(await response.Content.ReadAsStringAsync());
            var minLength = Math.Min(DbInitializer.SeedArticles.Count, result!.Count());
            TestHelper.AssertObjectEqual(
                DbInitializer.SeedArticles.GetRange(0, minLength),
                result!.ToList().GetRange(0, minLength));
            TestHelper.Print(_logger, result!);
        }

        [Fact]
        public async Task GetAdminArticles_Always_ReturnsAllArticles()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Articles/admin");
            request.Headers.Authorization = new AuthenticationHeaderValue(PassThroughAuthHandler.AuthenticationScheme);
            request.Headers.Add("UserId", "123");
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = JsonConvert.DeserializeObject<IEnumerable<Article>>(await response.Content.ReadAsStringAsync());
            var minLength = Math.Min(DbInitializer.SeedArticles.Count, result!.Count());
            TestHelper.AssertObjectEqual(
                DbInitializer.SeedArticles.GetRange(0, minLength),
                result!.ToList().GetRange(0, minLength));
            TestHelper.Print(_logger, result!);
        }

        [Fact]
        public async Task GetById_IfExists_ReturnsArticleWithTags()
        {

            var stubId = 1;
            var expectedResult = new ArticleWithTags
            {
                Id = stubId,
                Date = new DateTime(2022, 01, 01),
                Title = "NoSQL Review",
                Tags = new List<string?> { "Database", "MongoDb" }
            };

            var response = await _client.GetAsync($"api/Articles/{stubId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = JsonConvert.DeserializeObject<ArticleWithTags>(await response.Content.ReadAsStringAsync());
            TestHelper.AssertObjectEqual(expectedResult, result);
        }

        [Fact]
        public async Task GetById_IfMissing_Returns404()
        {
            var stubId = 10;
            var response = await _client.GetAsync($"api/Articles/{stubId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

    }

    [Collection("SequentialTests")]
    public class ArticlesPostTests
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ArticlesFixture _fixture;


        public ArticlesPostTests(ITestOutputHelper logger, ArticlesFixture fixture)
        {
            _logger = logger;
            _fixture = fixture;
            _client = _fixture.Client;

        }

        [Fact]
        public async Task PostArticle_WithValidData_ReturnsSavedArticle()
        {
            var stubId = 4;
            var expectedResult = new Article
            {
                Id = stubId,
                Date = new DateTime(2022, 01, 01),
                Title = "NewTitle",
            };

            var newArticle = new Article { Date = new DateTime(2022, 01, 01), Title = "NewTitle" }; //id auto generated
            var response = await _client.PostAsync("api/Articles", JsonContent.Create(newArticle));
            var result = JsonConvert.DeserializeObject<Article>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            TestHelper.AssertObjectEqual(expectedResult, result);
        }
    }

    [Collection("SequentialTests")]
    public class ArticlesPutTests
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ArticlesFixture _fixture;


        public ArticlesPutTests(ITestOutputHelper logger, ArticlesFixture fixture)
        {
            _logger = logger;
            _fixture = fixture;
            _client = _fixture.Client;
        }

        [Fact]
        public async Task PutArticle_WithValidData_ReturnsSavedArticle()
        {
            var stubId = 2;
            var expectedResult = new Article
            {
                Id = 2,
                Date = new DateTime(2022, 01, 01),
                Title = "NewTitle",
            };

            var newArticle = new Article { Id = stubId, Date = new DateTime(2022, 01, 01), Title = "NewTitle" };

            var response = await _client.PutAsync($"api/Articles/{stubId}", JsonContent.Create(newArticle));
            var result = JsonConvert.DeserializeObject<Article>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            TestHelper.AssertObjectEqual(expectedResult, result);
        }
    }

    [Collection("SequentialTests")]
    public class ArticlesDeleteTests
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ArticlesFixture _fixture;


        public ArticlesDeleteTests(ITestOutputHelper logger, ArticlesFixture fixture)
        {
            _logger = logger;
            _fixture = fixture;
            _client = _fixture.Client;
        }


        [Fact]
        public void DeleteArticle_WithValidData_ReturnsOK()
        {
            var stubId = 3;
            var deleteResponse = _client.DeleteAsync($"api/Articles/{stubId}").Result;
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}


