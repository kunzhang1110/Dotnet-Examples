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
    public class ArticlesControllerTestsCollectionDefinition { }

    [Collection("SequentialTests")]
    public class ArticlesControllerGetTests : IClassFixture<ApiExamplesContextFixture>
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;


        public ArticlesControllerGetTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {

            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;
            TestHelper.ReinitializeDbForTests(_contextFixture.Context);
        }

        [Fact]
        public async Task GetArticles_Always_ReturnsAllArticles()
        {
            var response = await _client.GetAsync("api/Articles");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = JsonConvert.DeserializeObject<IEnumerable<Article>>(await response.Content.ReadAsStringAsync());
            TestHelper.AssertObjectEqual(DbInitializer.SeedArticles, result!.ToList().GetRange(0, DbInitializer.SeedArticles.Count));
        }

        [Fact]
        public async Task GetById_IfExists_ReturnsArticleWithTags()
        {

            var studId = 1;

            var expectedResult = new ArticleWithTags
            {
                Date = new DateTime(2022, 01, 01),
                Title = "NoSQL Review",
                Tags = new List<string?> { "Database", "MongoDb" }
            };

            var response = await _client.GetAsync($"api/Articles/{studId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = JsonConvert.DeserializeObject<ArticleWithTags>(await response.Content.ReadAsStringAsync());
            TestHelper.Print(_logger, result);
            TestHelper.AssertObjectEqual(expectedResult, result);
        }

        [Fact]
        public async Task GetById_IfMissing_Returns404()
        {

            var studId = 10;
            var response = await _client.GetAsync($"api/Articles/{studId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    [Collection("SequentialTests")]
    public class ArticlesControllerPostTests : IClassFixture<ApiExamplesContextFixture>
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;


        public ArticlesControllerPostTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {

            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;
            TestHelper.ReinitializeDbForTests(_contextFixture.Context);
        }

        [Fact]
        public async Task PostArticle_WithValidData_ReturnsSavedArticle()
        {

            var expectedResult = new Article
            {
                Id = 4,
                Date = new DateTime(2022, 01, 01),
                Title = "NewTitle",
            };

            var newArticle = new Article { Date = new DateTime(2022, 01, 01), Title = "NewTitle" };

            var response = await _client.PostAsync("api/Articles", JsonContent.Create(newArticle));
            var result = JsonConvert.DeserializeObject<Article>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            TestHelper.AssertObjectEqual(expectedResult, result);
        }
    }

    [Collection("SequentialTests")]
    public class ArticlesControllerPutTests : IClassFixture<ApiExamplesContextFixture>
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;


        public ArticlesControllerPutTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {

            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;
            TestHelper.ReinitializeDbForTests(_contextFixture.Context);
        }

        [Fact]
        public async Task PutArticle_WithValidData_ReturnsSavedArticle()
        {
            var expectedResult = new Article
            {
                Id = 4,
                Date = new DateTime(2022, 01, 01),
                Title = "NewTitle",
            };

            var newArticle = new Article { Date = new DateTime(2022, 01, 01), Title = "NewTitle" };

            var response = await _client.PostAsync("api/Articles", JsonContent.Create(newArticle));
            var result = JsonConvert.DeserializeObject<Article>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            TestHelper.AssertObjectEqual(expectedResult, result);
        }
    }
    [Collection("SequentialTests")]
    public class ArticlesControllerDeleteTests : IClassFixture<ApiExamplesContextFixture>
    {
        private HttpClient _client;
        private readonly ITestOutputHelper _logger;
        private readonly ApiExamplesContextFixture _contextFixture;


        public ArticlesControllerDeleteTests(ITestOutputHelper logger, ApiExamplesContextFixture contextFixture)
        {

            _logger = logger;
            _contextFixture = contextFixture;
            _client = _contextFixture.Client;
            TestHelper.ReinitializeDbForTests(_contextFixture.Context);
        }


        [Fact]
        public  void  DeleteArticle_WithValidData_ReturnsOK()
        {
            var deleteResponse =  _client.DeleteAsync("api/Articles/1").Result;
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
            TestHelper.ReinitializeDbForTests(_contextFixture.Context);
            //var getAllResponse = await _client.GetAsync("api/Articles");

            //var result = JsonConvert.DeserializeObject<Article>(await getAllResponse.Content.ReadAsStringAsync());
            //TestHelper.Print(_logger, result);

            //TestHelper.AssertObjectEqual(modifiedArticle, result);
        }


    }
}


