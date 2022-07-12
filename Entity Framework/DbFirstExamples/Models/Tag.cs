using System;
using System.Collections.Generic;

namespace DbFirstExamples.Models
{
    public partial class Tag
    {
        public Tag()
        {
            ArticleTags = new HashSet<ArticleTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ArticleTag> ArticleTags { get; set; }
    }
}
