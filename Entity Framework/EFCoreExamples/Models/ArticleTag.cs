﻿namespace EFCoreExamples.Models
{
    public partial class ArticleTag
    {
        public int Id { get; set; }
        public int? ArticleId { get; set; }
        public int? TagId { get; set; }

        public virtual Article? Article { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
