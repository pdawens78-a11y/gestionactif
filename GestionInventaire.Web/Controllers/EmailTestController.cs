using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GestionInventaire.Web.Areas.Identity.Services;

namespace GestionInventaire.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly EmailSender _debugEmailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailTestController> _logger;

        public EmailTestController(
            IEmailSender emailSender,
            EmailSender debugEmailSender,
            IConfiguration configuration,
            ILogger<EmailTestController> logger)
        {
            _emailSender = emailSender;
            _debugEmailSender = debugEmailSender;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/emailtest/config
        /// Vérifier la configuration Mailjet
        /// </summary>
        [HttpGet("config")]
        public IActionResult CheckConfig()
        {
            var config = new
            {
                Provider = _configuration["Email:Provider"],
                Mailjet = new
                {
                    ApiKey = _configuration["Email:Mailjet:ApiKey"] != null ? "✅ Présent" : "❌ Absent",
                    ApiSecret = _configuration["Email:Mailjet:ApiSecret"] != null ? "✅ Présent" : "❌ Absent",
                    SenderEmail = _configuration["Email:Mailjet:SenderEmail"],
                    SenderName = _configuration["Email:Mailjet:SenderName"]
                },
                Message = "Configuration Mailjet chargée avec succès"
            };

            _logger.LogInformation("📋 Config Mailjet: {@Config}", config);
            return Ok(config);
        }

        /// <summary>
        /// POST /api/emailtest/send
        /// Envoyer un email de test
        /// Body: { "to": "email@example.com", "subject": "Test", "body": "Contenu test" }
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail([FromBody] EmailTestRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.To))
            {
                return BadRequest(new { error = "Email 'to' requis" });
            }

            _logger.LogInformation("🧪 Test email à {Email}...", request.To);

            try
            {
                var subject = request.Subject ?? "Email de Test";
                var body = request.Body ?? "<h1>Email de test</h1><p>Si vous recevez ceci, Mailjet fonctionne!</p>";

                await _emailSender.SendEmailAsync(request.To, subject, body);

                _logger.LogInformation("✅ Envoi complété pour {Email}", request.To);
                return Ok(new
                {
                    message = "Email envoyé avec succès",
                    to = request.To,
                    subject = subject,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur envoi email de test à {Email}", request.To);
                return StatusCode(500, new
                {
                    error = "Erreur lors de l'envoi",
                    details = ex.Message,
                    exception = ex.GetType().Name
                });
            }
        }

        /// <summary>
        /// GET /api/emailtest/send?to=email@example.com&subject=Test&body=Hello
        /// Version GET pour tester simplement
        /// </summary>
        [HttpGet("send")]
        public async Task<IActionResult> SendTestEmailGet(
            [FromQuery] string to,
            [FromQuery] string subject = "Email de Test",
            [FromQuery] string body = "<h1>Email de test</h1><p>Si vous recevez ceci, Mailjet fonctionne!</p>")
        {
            if (string.IsNullOrEmpty(to))
            {
                return BadRequest(new { error = "Paramètre 'to' requis: /api/emailtest/send?to=email@example.com" });
            }

            _logger.LogInformation("🧪 Test email GET à {Email}...", to);

            try
            {
                await _emailSender.SendEmailAsync(to, subject, body);

                _logger.LogInformation("✅ Envoi GET complété pour {Email}", to);
                return Ok(new
                {
                    message = "✅ Email envoyé avec succès!",
                    to = to,
                    subject = subject,
                    timestamp = DateTime.UtcNow,
                    note = "Vérifiez votre boîte mail et les spams"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur envoi email GET à {Email}", to);
                return StatusCode(500, new
                {
                    error = "❌ Erreur lors de l'envoi",
                    details = ex.Message,
                    exception = ex.GetType().Name,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// GET /api/emailtest/debug-send?to=email@example.com
        /// Retourne la réponse brute Mailjet pour diagnostiquer la livraison
        /// </summary>
        [HttpGet("debug-send")]
        public async Task<IActionResult> DebugSendEmailGet(
            [FromQuery] string to,
            [FromQuery] string subject = "Email de Test Debug",
            [FromQuery] string body = "<h1>Email de test</h1><p>Si vous recevez ceci, Mailjet fonctionne!</p>")
        {
            if (string.IsNullOrEmpty(to))
            {
                return BadRequest(new { error = "Paramètre 'to' requis: /api/emailtest/debug-send?to=email@example.com" });
            }

            _logger.LogInformation("🧪 Debug email à {Email}...", to);

            try
            {
                var result = await _debugEmailSender.SendEmailDebugAsync(to, subject, body);
                _logger.LogInformation("📨 Résultat debug Mailjet pour {Email}: {Result}", to, result);

                return Ok(new
                {
                    to,
                    subject,
                    result,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur debug envoi email à {Email}", to);
                return StatusCode(500, new
                {
                    error = "❌ Erreur lors du debug",
                    details = ex.Message,
                    exception = ex.GetType().Name,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// GET /api/emailtest/logs
        /// Afficher les derniers logs du service email
        /// </summary>
        [HttpGet("logs")]
        public IActionResult GetLogs()
        {
            return Ok(new
            {
                message = "Vérifiez la console de debug (Debug Console) pour les logs détaillés",
                logSearchFor = "📧 ou ✅ ou ❌",
                debugConsole = "View → Debug Console (Ctrl+Shift+Y)"
            });
        }
    }

    public class EmailTestRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
