using ApiExamples.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ApiExamples.Utils
{
    public static class DbInitializer
    {
        private static readonly List<Article> _seedArticles = new()
        {
                new() {  Date=new DateTime(2022,1,1), Title="NoSQL Review",Viewed=2},
                new() {  Date=new DateTime(2022,1,2), Title="Python Review",Viewed=5},
                new() { Date = new DateTime(2022, 1, 2), Title = "Financial Analysis", Viewed = 10 },
        };

        private static readonly List<Tag> _seedTags = new()
        {
               new() {  Name="Database"},
               new() {  Name="MongoDb"},
               new() {  Name="Python"},
               new() {  Name="Finance"}
            };

        public static List<Article> SeedArticles { get => _seedArticles; }
        public static List<Tag> SeedTags { get => _seedTags; }

        public static ArticleTag? CreateArticleTag(string articleName, string tagName, ApiExamplesContext context)
        {
            var articleId = context.Articles.Where(a => a.Title == articleName).First()?.Id;
            var tagId = context.Tags.Where(t => t.Name == tagName).First()?.Id;

            if (articleId == null || tagId == null) return null;


            return new ArticleTag
            {
                ArticleId = articleId,
                TagId = tagId
            };
        }

        public static async Task Initialize(ApiExamplesContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "a",
                    Email = "a@a.com"
                };

                await userManager.CreateAsync(user, "Zk000000!");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@a.com"
                };

                await userManager.CreateAsync(admin, "Zk000000!");
                await userManager.AddToRolesAsync(admin, new string[] { "Member", "Admin" });

            }

            if (context.Articles.Any()) return;



            foreach (var article in _seedArticles)
            {
                context.Articles.Add(article);
            }

            foreach (var tag in _seedTags)
            {
                context.Tags.Add(tag);
            }

            context.SaveChanges();

            var articleTags = new List<ArticleTag?>
            {
                CreateArticleTag("NoSQL Review","Database",context),
                CreateArticleTag("NoSQL Review","MongoDb",context),
                CreateArticleTag("Python Review","Python",context),
                CreateArticleTag("Financial Analysis","MongoDb",context),
                CreateArticleTag("Financial Analysis","Python",context),
                CreateArticleTag("Financial Analysis","Finance",context),

            };
            foreach (var at in articleTags)
            {
                if (at != null) context.ArticleTags.Add(at);

            }
            context.SaveChanges();
        }
    }
}