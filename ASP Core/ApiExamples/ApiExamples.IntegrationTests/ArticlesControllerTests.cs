using ApiExamples.Controllers;
using ApiExamples.Models;
using ApiExamples.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;


using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiExamples.IntegrationTests
{

    [CollectionDefinition("SequentialTests", DisableParallelization = true)]
    public class ArticlesControllerTestsCollectionDefinition : ICollectionFixture<ApiExamplesContextFixture> { }

    [Collection("SequentialTests")]
    public class ArticlesControllerGetTests
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;

        public ArticlesControllerGetTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {
            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;
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
    public class ArticlesControllerPostTests
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;


        public ArticlesControllerPostTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {
            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;

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
    public class ArticlesControllerPutTests
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;


        public ArticlesControllerPutTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {
            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;
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
    public class ArticlesControllerDeleteTests
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;


        public ArticlesControllerDeleteTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {

            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;
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


