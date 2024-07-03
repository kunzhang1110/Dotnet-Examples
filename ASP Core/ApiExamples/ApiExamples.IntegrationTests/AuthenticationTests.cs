using ApiExamples.DTOs;
using ApiExamples.Models;
using ApiExamples.Utils;

using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace ApiExamples.IntegrationTests
{
    public class AuthenticationTests : IClassFixture<AuthenticationFixture>
    {
        private readonly ITestOutputHelper _logger;
        private readonly AuthenticationFixture _fixture;
        private readonly HttpClient _client;

        public AuthenticationTests(ITestOutputHelper logger, AuthenticationFixture fixture)
        {
            _logger = logger;
            _fixture = fixture;
            _client = _fixture.Client;
        }

        [Fact]
        public async Task RegisterLoginUseSession_Always_ReturnsSessionString()
        {
            var stubUsername = "Username";
            var stubEmail = "a@a.com";
            var stubPassword = "Test000000!";


            var stubRegisterFormData = new MultipartFormDataContent
            {
                { new StringContent(stubUsername), "Username" },
                { new StringContent(stubPassword), "Password" },
                { new StringContent(stubEmail) , "Email" }
            };

            var registerResponse = await _client.PostAsync("api/Authentication/register", stubRegisterFormData);
            var registerResponseUser = JsonConvert.DeserializeObject<User>(await registerResponse.Content.ReadAsStringAsync());

            if (registerResponse.IsSuccessStatusCode)
            {
                Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
                Assert.NotNull(registerResponseUser);
                Assert.Equal(stubUsername, registerResponseUser.UserName);
                Assert.Equal(stubEmail, registerResponseUser.Email);
            }
            else
            {
                TestHelper.Print(_logger, await registerResponse.Content.ReadAsStringAsync());
                Assert.Fail("Api call failed.");
            }

            var loginResponse = await _client.PostAsync("api/Authentication/login", stubRegisterFormData);
            var loginResponseUserDto = JsonConvert.DeserializeObject<UserDto>(await loginResponse.Content.ReadAsStringAsync());
            var token = ""; //JWT token
            if (registerResponse.IsSuccessStatusCode)
            {
                Assert.NotNull(loginResponseUserDto);
                token = loginResponseUserDto.Token;
                Assert.Equal(stubEmail, loginResponseUserDto.Email);
            }
            else
            {
                TestHelper.Print(_logger, await registerResponse.Content.ReadAsStringAsync());
                Assert.Fail("Api call failed.");
            }

            var stubUserInput = "Dummy";

            var startSessionFormContent = new MultipartFormDataContent
            {
                { new StringContent(stubUserInput), "UserInput" },
            };

            var startSessionResponse =
                await _client.PostAsync(_client.BaseAddress + "api/Authentication/startSession", startSessionFormContent);
            var cookies = startSessionResponse.Headers.GetValues("Set-Cookie");

            var useSessionRequest =
                new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "api/Authentication/useSession");
            useSessionRequest.Headers.Authorization
                = new AuthenticationHeaderValue("Bearer", token);

            foreach (var cookie in cookies)
            {//set cookies
                useSessionRequest.Headers.Add("Cookie", cookie);
            }
            var useSessionResponse = await _client.SendAsync(useSessionRequest);

            if (registerResponse.IsSuccessStatusCode)
            {
                var responseBody = await useSessionResponse.Content.ReadAsStringAsync();
                Assert.Equal($"{stubUserInput} blue", responseBody);
            }
            else
            {
                TestHelper.Print(_logger, useSessionResponse.Content);
                Assert.Fail("Api call failed.");
            }

        }
    }
}


