
using EFCoreExamples.Models;

namespace EFCoreExamples
{
    public static class DbInitializer
    {
        private static ArticleTag? CreateArticleTag(string articleName, string tagName, EFCoreExamplesContext context)
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

        public static void Initialize(EFCoreExamplesContext context)
        {

            if (context.Articles.Any()) return;

            var articles = new List<Article>
            {
                new() {  Date=new DateTime(2022,1,1), Title="NoSQL Review",Viewed=2},
                new() {  Date=new DateTime(2022,1,2), Title="Python Review",Viewed=5},
                new() {  Date=new DateTime(2022,1,2), Title="Financial Analysis",Viewed=10},
            };


            var tags = new List<Tag>
            {
                new () {  Name="Database"},
                new () {  Name="MongoDb"},
                new () {  Name="Python"},
                new () {  Name="Finance"}
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