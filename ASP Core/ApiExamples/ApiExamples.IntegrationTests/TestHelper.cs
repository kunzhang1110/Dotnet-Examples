using ApiExamples.Models;
using ApiExamples.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Abstractions;

namespace ApiExamples.IntegrationTests
{
    public static class TestHelper
    {
        public static void InitializeDbForTests(ApiExamplesContext context)
        {
            context.Articles.AddRange(DbInitializer.SeedArticles);
            context.SaveChanges();
            context.Tags.AddRange(DbInitializer.SeedTags);
            context.SaveChanges();
            var articleTags = new List<ArticleTag?>
            {
                DbInitializer.CreateArticleTag("NoSQL Review","Database",context),
                DbInitializer.CreateArticleTag("NoSQL Review","MongoDb",context),
                DbInitializer.CreateArticleTag("Python Review","Python",context),
                DbInitializer.CreateArticleTag("Financial Analysis","MongoDb",context),
                DbInitializer.CreateArticleTag("Financial Analysis","Python",context),
                DbInitializer.CreateArticleTag("Financial Analysis","Finance",context),

            };
            foreach (var at in articleTags)
            {
                if (at != null) context.ArticleTags.Add(at);

            }
            context.SaveChanges();
        }

        public static void EraseDbForTests(ApiExamplesContext context)
        {
            context.ArticleTags.RemoveRange(context.ArticleTags);
            context.SaveChanges();
            context.Tags.RemoveRange(context.Tags);
            context.SaveChanges();
            context.Articles.RemoveRange(context.Articles);
            context.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApiExamplesContext context)
        {

            EraseDbForTests(context);
            InitializeDbForTests(context);
        }

        public static string ObjectToString(object? obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            });
        }

        public static void Print(ITestOutputHelper output, object obj)
        {
            output.WriteLine(JsonSerializer.Serialize(obj,
                 new JsonSerializerOptions { WriteIndented = true }));
            output.WriteLine(obj.GetType().ToString());
        }

        public static void AssertObjectEqual(object expected, object? actual)
        {
            Assert.Equal(ObjectToString(expected), ObjectToString(actual));
        }
    }
}
