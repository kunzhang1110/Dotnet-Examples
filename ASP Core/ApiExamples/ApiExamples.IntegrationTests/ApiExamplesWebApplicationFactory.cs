using ApiExamples.Models;
using ApiExamples.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Data.Common;


namespace ApiExamples.IntegrationTests
{
    public class ApiExamplesWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IArticlesRepository> StubArticleRepo { get; }

        public ApiExamplesWebApplicationFactory()
        {
            StubArticleRepo = new Mock<IArticlesRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");

            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                     d => d.ServiceType ==
                  typeof(DbContextOptions<ApiExamplesContext>));
                services.Remove(dbContextDescriptor!);//remove existing DB service

                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<ApiExamplesContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

        
            });
        }
    }
}
