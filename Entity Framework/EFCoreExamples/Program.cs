using EFCoreExamples;
using EFCoreExamples.Models;
using Microsoft.EntityFrameworkCore;


var context = new EFCoreExamplesContext("Server=MY-LEGION;Database=EFCoreExamples;Trusted_Connection=True;Encrypt=false");
await context.Database.MigrateAsync(); //apply migrations
DbInitializer.Initialize(context);

