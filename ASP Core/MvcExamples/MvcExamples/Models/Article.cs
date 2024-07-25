using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcExamples.Models
{
    public partial class Article
    {
        public Article()
        {
            ArticleTags = new HashSet<ArticleTag>();
        }

        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; } = null!;
        public int? Viewed { get; set; }

        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
    }
}
