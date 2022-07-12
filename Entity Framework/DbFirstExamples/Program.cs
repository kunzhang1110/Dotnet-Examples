using DbFirstExamples.Models;

var context = new DbFirstExamplesContext();
var articles = context.Articles.ToList();
foreach (var article in articles) { Console.WriteLine(article.Title); }
