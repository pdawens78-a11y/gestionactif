using GestionInventaire.Web.Areas.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Route("debug/email")]
    public class DebugEmailController : Controller
    {
        private readonly EmailSender _emailSender;

        public DebugEmailController(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet("send")]
        public async Task<IActionResult> Send([FromQuery] string to, [FromQuery] string subject = "Test email", [FromQuery] string body = "<p>Test email from GestionInventaire</p>")
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                return BadRequest("Query parameter 'to' is required.");
            }

            var result = await _emailSender.SendEmailDebugAsync(to, subject, body);
            return Content(result, "text/plain");
        }
    }
}
