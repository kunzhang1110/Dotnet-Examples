using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApiExamples.IntegrationTests
{
    // Pass throught [Authorization] with TestUser, Admin as claims
    public class PassThroughAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public const string AuthenticationScheme = "TestScheme";

        public PassThroughAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                    ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock
                ) : base(options, logger, encoder, clock) { }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("No authorization header"));

            // create new Claim to be passed to the next middleware
            // Mock as if the user have already logged in with the new claims
            var claims = new List<Claim>{
            new (ClaimTypes.Name, "TestUser"),
            new (ClaimTypes.Role, "admin")
            };
            if (Context.Request.Headers.TryGetValue("UserId", out var userId))
            {
                claims.Add(new(ClaimTypes.NameIdentifier, userId[0]));
            }
            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
