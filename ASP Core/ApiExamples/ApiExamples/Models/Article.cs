using ApiExamples.Shared;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiExamples.Models
{
    public partial class Article
    {
        public Article()
        {
            ArticleTags = new HashSet<ArticleTag>();
        }

        public int Id { get; set; }

        [CustomDateValidation("2000-01-01", EndDate = "2025-01-01", ErrorMessage = "Date is not in range")]
        public DateTime? Date { get; set; }

        public string Title { get; set; } = null!;
        public int? Viewed { get; set; }

        [JsonIgnore]
        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
    }
}
