using Microsoft.AspNetCore.Mvc;
using ApiExamples.Models;
using Microsoft.AspNetCore.Identity;
using ApiExamples.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiExamples.Data;
using Microsoft.AspNetCore.Authorization;

namespace ApiExamples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ApiExamplesContext _context;

        public AuthenticationController(UserManager<User> userManager, ApiExamplesContext context, IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
        }

        private async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:TokenKey"]));

            var tokenOptions = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromForm] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            return new UserDto
            {
                Email = user.Email,
                Token = await GenerateToken(user),

            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Username, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();

            }

            await _userManager.AddToRoleAsync(user, "Member");

            return CreatedAtAction("register", user);

        }


        [HttpPost("startSession")]
        public async Task<ActionResult> StartSession([FromForm] string userInput)
        {
            var sessionId = Guid.NewGuid().ToString(); //random id

            var cookieOptions = new CookieOptions
            {
                //Secure = true,
                IsEssential = true,//if false subquent requests in test will not get cookies
                Expires = DateTime.Now.AddDays(30),
                SameSite = SameSiteMode.None
            };

            _context.Sessions.Add(new Session { Id = sessionId, Detail = userInput }); //persist session id and data in db
            await _context.SaveChangesAsync();

            Response.Cookies.Append("sessionId", sessionId, cookieOptions);
            Response.Cookies.Append("color", "blue", cookieOptions);

            return Ok();
        }

        [Authorize]
        [HttpGet("useSession")]
        public async Task<ActionResult> UseSession()
        {
            var sessionId = Request.Cookies["sessionId"];
            var color = Request.Cookies["color"];
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null)
            {
                return Ok("Session not found");
            }
            return Ok(session.Detail + " " + color);
        }

    }
}
