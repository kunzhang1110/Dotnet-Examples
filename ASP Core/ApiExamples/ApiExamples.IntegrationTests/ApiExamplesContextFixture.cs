using ApiExamples.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;


namespace ApiExamples.IntegrationTests
{
    public class ApiExamplesContextFixture : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        public ApiExamplesContext Context { get; private set; }
        public HttpClient Client { get; private set; }

        public ApiExamplesContextFixture()
        {
            _factory = new CustomWebApplicationFactory();
            using (var scope = _factory.Services.CreateScope())
            {
                Client = _factory.CreateClient();
                var scopedServices = scope.ServiceProvider;
                Context = scopedServices.GetRequiredService<ApiExamplesContext>();
                Context.Database.EnsureCreated();
                TestHelper.InitializeDbForTests(Context);
            }

        }

        public void Dispose()
        {
            //TestHelper.EraseDbForTests(Context);
        }

    }

}
