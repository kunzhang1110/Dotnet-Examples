using EFCoreExamples.Models;
using System.Text.Json;


namespace EFCoreExamples.Test
{
    public class EFCoreExamplesContextTests
    {
        private readonly EFCoreExamplesContext _context;
        private readonly ITestOutputHelper _output;
        private readonly string _connectionString = "Server=MY-LEGION;Database=EFCoreExamples;Trusted_Connection=True;Encrypt=false";

        public EFCoreExamplesContextTests(ITestOutputHelper output)
        {
            _output = output;
            _context = new EFCoreExamplesContext(_connectionString);
        }


        [Fact]
        public void DbContextExample()
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

    }
}