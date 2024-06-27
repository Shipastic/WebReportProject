using DAPManSWebReports.Domain.ErrorReportService;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/senderror")]
    [ApiController]
    public class SendErrorController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public SendErrorController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        // GET: api/<SendErrorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SendErrorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SendErrorController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ReportError reportError)
        {
            if (reportError == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                await _emailService.SendErrorReportAsync(reportError);
                return Ok("Error report sent successfully.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while sending the error report.");
            }
        }

        // PUT api/<SendErrorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SendErrorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
