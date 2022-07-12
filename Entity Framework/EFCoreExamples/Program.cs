using EFCoreExamples;
using EFCoreExamples.Models;
using EFCoreExamples.Repository;
using Microsoft.EntityFrameworkCore;


var context = new EFCoreExamplesContext();
await context.Database.MigrateAsync(); //apply migrations
DbInitializer.Initialize(context);

