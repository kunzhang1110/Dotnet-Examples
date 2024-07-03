using ApiExamples.Data;
using ApiExamples.Models;
using ApiExamples.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ApiExamples.IntegrationTests
{

    public class AuthenticationFixture
    {
        private readonly AuthenticationWebApplicationFactory _factory;
        public ApiExamplesContext DbContext { get; private set; }
        public HttpClient Client { get; private set; }

        public AuthenticationFixture()
        {
            _factory = new AuthenticationWebApplicationFactory();
            Client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            DbContext = scopedServices.GetRequiredService<ApiExamplesContext>();
            DbContext.Database.EnsureCreated();
        }

    }

}
