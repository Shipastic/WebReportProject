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

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (userLogin.Username is not null && userLogin.Password is not null)
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey   = jwtSettings["SecretKey"];
                var issuer      = jwtSettings["Issuer"];
                var audience    = jwtSettings["Audience"];
                var role        = userLogin.Username;
                try
                {
                    var token   = await _jwtHandler.GetTokenAsync(userLogin.Username, secretKey, issuer, audience, role);
                    var response = new UserResponse
                    {
                        Username = userLogin.Username,
                        Role     = TokenDecoder.GetRoleFromToken(token, secretKey, issuer, audience),
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

        [HttpPost("token")]
        public async Task<IActionResult> GetTokenKeyCloak([FromBody] UserLogin userLogin)
        {
            var jwtSettings  = _configuration.GetSection("KeyCloakSettings");
            var clientId     = jwtSettings["client_id"];
            var clientSecret = jwtSettings["client_secret"];
            var issuer       = jwtSettings["Issuer"];
            var audience     = jwtSettings["Audience"];
            try
            {
                var token    = await _jwtHandler.GetTokenAsync(userLogin.Username, userLogin.Password, clientId, clientSecret);
                var claims   = TokenUtility.ValidateToken(token, clientSecret, issuer, audience);
                var response = new UserResponse
                {
                    Username = userLogin.Username,
                    Role     = TokenDecoder.GetRoleFromToken(token, clientSecret, issuer, audience),
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
    }
}
