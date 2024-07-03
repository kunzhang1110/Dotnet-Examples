using ApiExamples.Data;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Data.Common;

namespace ApiExamples.IntegrationTests
{
    public class AuthenticationWebApplicationFactory : WebApplicationFactory<Program>
    {
        public AuthenticationWebApplicationFactory() { }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");//in Program.cs builder.Environment.IsEnvironment("Test")

            builder.ConfigureTestServices(services =>
            {
                //remove existing DB service
                var dbContextDescriptor = services.SingleOrDefault(
                     d => d.ServiceType ==
                  typeof(DbContextOptions<ApiExamplesContext>));
                services.Remove(dbContextDescriptor!);

                // Add DbConnection
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    return connection;
                });

                // Add DbContext using DbConnection
                services.AddDbContext<ApiExamplesContext>((serviceProvider, options) =>
                {
                    var connection = serviceProvider.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

            });
        }
    }
}
