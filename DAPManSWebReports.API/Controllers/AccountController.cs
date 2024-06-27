using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //private readonly ModelContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly JwtHandler _jwtHandler;

        //public AccountController(ModelContext context, UserManager<ApplicationUser> userManager, JwtHandler jwtHandler)
        //{
        //    _context = context;
        //    _userManager = userManager;
        //    _jwtHandler = jwtHandler;
        //}

        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        //[HttpPost("Login")]
        //public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
        //{
        //    var user = await _userManager.FindByNameAsync(loginRequest.Email);
        //    if (user == null
        //    || !await _userManager.CheckPasswordAsync(user, loginRequest.
        //   Password))
        //        ///Выдаем результат в случае неуспешного входа
        //        return Unauthorized(new LoginResult()
        //        {
        //            Success = false,
        //            Message = "Invalid Email or Password."
        //        });

        //    var secToken = await _jwtHandler.GetTokenAsync(user);
        //    //создаем json-представление токена
        //    var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
        //    //Выдаем результат в случае успеха
        //    return Ok(new LoginResult()
        //    {
        //        Success = true,
        //        Message = "Login successful",
        //        Token = jwt
        //    });
        //}

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
