using EFCoreExamples.Models;
using EFCoreExamples.Repository;
using System.Text.Json;

namespace EFCoreExamples.Test
{
    public class RepositoryExamplesTests
    {
        private readonly EFCoreExamplesContext _context;
        private readonly ITestOutputHelper _output;
        private readonly string _connectionString = "Server=MY-LEGION;Database=EFCoreExamples;Trusted_Connection=True;Encrypt=false";

        public RepositoryExamplesTests(ITestOutputHelper output)
        {
            _output = output;
            _context = new EFCoreExamplesContext(_connectionString);
        }

        [Fact]
        public void ArticleRepoExample()
        {
            var aritcleRepo = new ArticleRepository(_context);
            aritcleRepo.Add(new Article()
            {
                Date = new DateTime(2022, 1, 3),
                Title = "Repository Pattern",
                Viewed = 3
            });
            aritcleRepo.SaveChanges();
            var article = aritcleRepo.GetFirstByTitle("Repository Pattern");
            _output.WriteLine(JsonSerializer.Serialize(article));
            aritcleRepo.Delete(article);
            aritcleRepo.SaveChanges();
        }

        [Fact]
        public void GenericRepoExample()
        {
            var genericRepo = new GenericRepository<Article>(_context);
            genericRepo.Add(new Article()
            {
                Date = new DateTime(2022, 1, 3),
                Title = "Repository Pattern - Generic",
                Viewed = 3
            });
            genericRepo.SaveChanges();
            genericRepo.GetAll().ForEach(x => _output.WriteLine(x.Title));
            _context.Articles.RemoveRange(_context.Articles.Where(a => a.Title == "Repository Pattern - Generic"));
            genericRepo.SaveChanges();
        }

        [Fact]
        public void UnitOfWorkExample()
        {
            var uow = new UnitOfWork(_context);
            uow.ArticleRepository.Add(new Article()
            {
                Date = new DateTime(2022, 1, 3),
                Title = "Unit of Work Pattern",
                Viewed = 3
            });
            uow.Save();
            uow.ArticleRepository.GetAll().ForEach(x => _output.WriteLine(x.Title));
            uow.TagRepository.GetAll().ForEach(x => _output.WriteLine(x.Name));
            _context.Articles.RemoveRange(_context.Articles.Where(a => a.Title == "Unit of Work Pattern"));
            uow.Save();
        }

    }
}