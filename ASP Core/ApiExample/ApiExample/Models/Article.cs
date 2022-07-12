namespace ApiExample.Models
{
    public partial class Article
    {
        public Article()
        {
            ArticleTags = new HashSet<ArticleTag>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Title { get; set; } = null!;
        public int? Viewed { get; set; }

        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
    }
}
