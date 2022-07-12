using System;
using System.Collections.Generic;

namespace ApiExample.Models
{
    public partial class ArticleWithTags
    {
        public DateTime? Date { get; set; }
        public string Title { get; set; } = null!;

        public ICollection<string?>? Tags { get; set; }
    }
}
