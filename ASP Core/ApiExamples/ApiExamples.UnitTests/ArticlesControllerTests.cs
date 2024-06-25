using Xunit.Abstractions;
using ApiExamples.Models;
using ApiExamples.Repositories;
using Moq;
using ApiExamples.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace ApiExamples.UnitTests
{
    public class ArticlesControllerTests
    {

        private readonly ITestOutputHelper _output;
        Mock<ILogger<ArticlesController>> _stubLogger;

        public ArticlesControllerTests(ITestOutputHelper output)
        {
            _stubLogger = new Mock<ILogger<ArticlesController>>();
            _output = output;
        }


        [Fact]
        public async void GetById_IfExists_ReturnsArticleWithTags()
        {
            var stubArticleWithTag = new ArticleWithTags
            {
                Date = new DateTime(2022, 1, 1),
                Title = "NoSQL Review",
                Viewed = 2,
                Tags = new List<string?> { "Database", "MongoDb" }
            };


            var stubRepository = new Mock<IArticlesRepository>();
            var stubId = 1;

            stubRepository.Setup(r => r.GetArticleWithTagByIdAsync(It.IsAny<int>())).ReturnsAsync(stubArticleWithTag);

            var controller = new ArticlesController(stubRepository.Object, _stubLogger.Object);

            var result = await controller.GetArticle(stubId);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ArticleWithTags>(okObjectResult.Value);
            TestHelper.AssertObjectEqual(stubArticleWithTag, returnValue);
        }

        [Fact]
        public async void GetById_IfMissing_Returns404()
        {
            var stubRepository = new Mock<IArticlesRepository>();
            var stubId = 1;

            stubRepository.Setup(r => r.GetArticleWithTagByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ArticleWithTags?)null)
                .Verifiable(Times.Once);

            var controller = new ArticlesController(stubRepository.Object, _stubLogger.Object);
            var result = await controller.GetArticle(stubId);
            Assert.IsType<NotFoundResult>(result.Result);
            stubRepository.VerifyAll();
        }


        [Fact]
        public async  void Post_WithValidData_SavesArticle()
        {
            var stubArticle = new Article
            {
                Date = new DateTime(2022, 1, 1),
                Title = "NoSQL Review"
            };

            var stubRepository = new Mock<IArticlesRepository>();

            stubRepository
                .Setup(r => r.CreateArticleAsync(It.Is<Article>(a => a.Date == new DateTime(2022, 1, 1) && a.Title == "NoSQL Review")))
                .ReturnsAsync(stubArticle)
                .Verifiable(Times.Once);

            var controller = new ArticlesController(stubRepository.Object, _stubLogger.Object);

            var result = await controller.PostArticle(stubArticle);
            
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Article>(createdAtActionResult.Value);
            TestHelper.AssertObjectEqual(stubArticle, returnValue);
            stubRepository.VerifyAll();
        }


    }
}