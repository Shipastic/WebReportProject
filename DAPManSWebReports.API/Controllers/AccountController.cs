using DAPManSWebReports.Domain.IdentityService;
using DAPManSWebReports.Domain.IdentityService.TokenServise;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly JwtHandler _jwtHandler;

        public AccountController(IConfiguration configuration)
        {
           _configuration = configuration ?? throw new ArgumentNullException(nameof(_configuration));
            _jwtHandler = new JwtHandler();
        }

        // POST api/<AccountController>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (userLogin.Username is not null && userLogin.Password is not null)
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey   = jwtSettings["SecretKey"];
                var role        = userLogin.Username;
                try
                {
                    var token   = await _jwtHandler.GetTokenAsync(userLogin.Username, secretKey, jwtSettings["Issuer"], jwtSettings["Audience"], role);
                    var response = new UserResponse
                    {
                        Username = userLogin.Username,
                        Role     = role,
                        Token    = token
                    };
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return BadRequest(ex);
                }

            }
            return Unauthorized();
        }
    }
}
