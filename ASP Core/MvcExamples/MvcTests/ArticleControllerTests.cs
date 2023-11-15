using MvcExamples.Models;
using MvcExamples.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MvcTest
{
    public class ArticleControllerTests
    {
        [Fact]
        public async void Create_WhenModelStateIsValid_ReturnsRedirectToAction()
        {
            var mockContext = new Mock<MvcExamplesContext>("");

            mockContext.Setup((context) => context.SaveChangesAsync(default))
            .Returns(Task.FromResult(1)).Verifiable();
            var controller = new ArticlesController(mockContext.Object);

            var newArticle = new Article
            {
                Id = 1,
                Title = "SQL"
            };

            var result = await controller.Create(newArticle);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Create", redirectToActionResult.ActionName);
            mockContext.Verify();
        }
    }
}