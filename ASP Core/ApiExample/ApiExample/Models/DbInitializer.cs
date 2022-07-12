
using Microsoft.AspNetCore.Identity;

namespace ApiExample.Models
{
    public static class DbInitializer
    {
        private static ArticleTag? CreateArticleTag(string articleName, string tagName, ApiExampleContext context)
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

        public static async Task Initialize(ApiExampleContext context, UserManager<User> userManager)
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

            var articles = new List<Article>
            {
                new Article {  Date=new DateTime(2022,1,1), Title="NoSQL Review",Viewed=2},
                new Article {  Date=new DateTime(2022,1,2), Title="Python Review",Viewed=5},
                new Article {  Date=new DateTime(2022,1,2), Title="Financial Analysis",Viewed=10},
            };


            var tags = new List<Tag>
            {
                new Tag {  Name="Database"},
                new Tag {  Name="MongoDb"},
                new Tag {  Name="Python"},
                new Tag {  Name="Finance"}
            };

            foreach (var article in articles)
            {
                context.Articles.Add(article);
            }

            foreach (var tag in tags)
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