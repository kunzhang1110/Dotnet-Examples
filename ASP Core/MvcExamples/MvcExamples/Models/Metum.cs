using System;
using System.Collections.Generic;

namespace MvcExamples.Models
{
    public partial class Metum
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Value { get; set; }
        public string? Text { get; set; }
    }
}
