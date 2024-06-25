using System;
using System.Collections.Generic;

namespace ApiExamples.Models
{
    public partial class ArticleWithTags
    {
        public DateTime? Date { get; set; }
        public string Title { get; set; } = null!;
        public int? Viewed { get; set; }
        public ICollection<string?>? Tags { get; set; }
    }
}
