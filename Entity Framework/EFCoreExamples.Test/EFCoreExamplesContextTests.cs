using EFCoreExamples.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace EFCoreExamples.Test
{
    public class EFCoreExamplesContextTests
    {
        private readonly EFCoreExamplesContext _context;
        private readonly ITestOutputHelper _output;


        public EFCoreExamplesContextTests(ITestOutputHelper output)
        {
            _output = output;
            var databaseFilePath = Path.Combine("C:\\GitHub\\Examples\\Dotnet-Examples\\Entity Framework\\EFCoreExamples.Test", "test.db");
            _context = new EFCoreExamplesContext($"Data Source={databaseFilePath}", true);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            DbInitializer.Initialize(_context);
        }


        [Fact]
        public void AddArticle()
        {
            _context.Articles.Add(new Article()
            {
                Date = new DateTime(2022, 1, 2),
                Title = "New Article",
                Viewed = 2
            });
            _context.SaveChanges();
            var article = _context.Articles.Where(a => a.Title == "New Article").First();
            _output.WriteLine(JsonSerializer.Serialize(article));
            _context.Articles.Remove(article);
            _context.SaveChanges();
        }

        [Fact]
        public void UpdateArticleFromExisting()
        {
            var existingArticle = _context.Articles.Where(a => a.Id == 1).First();
            existingArticle.Title = "UpdateArticleFromExisting Article";
            _context.SaveChanges();
            var article = _context.Articles.Where(a => a.Id == 1).First();
            _output.WriteLine(JsonSerializer.Serialize(article));

        }

        [Fact]
        public void UpdateArticleFromNew()
        {
            var newUpdatedArticle = new Article()
            {
                Id = 1,
                Date = new DateTime(2024, 1, 2),
                Title = "UpdateArticleFromNew Article",
                Viewed = 2
            };

            _context.Articles.Update(newUpdatedArticle);
            _context.SaveChanges();
            var article = _context.Articles.Where(a => a.Id == 1).First();
            _output.WriteLine(JsonSerializer.Serialize(article));

        }

        [Fact]
        public void UpdateArticleByModifyingState()
        {
            var newUpdatedArticle = new Article()
            {
                Id = 1,
                Date = new DateTime(2024, 1, 2),
                Title = "UpdateArticleByModifyingState Article",
                Viewed = 3
            };

            _context.Entry(newUpdatedArticle).State = EntityState.Modified;
            _context.SaveChanges();
            var article = _context.Articles.Where(a => a.Id == 1).First();
            _output.WriteLine(JsonSerializer.Serialize(article));

        }

    }
}