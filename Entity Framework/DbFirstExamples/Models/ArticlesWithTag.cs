using System;
using System.Collections.Generic;

namespace DbFirstExamples.Models
{
    public partial class ArticlesWithTag
    {
        public DateTime? Date { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
