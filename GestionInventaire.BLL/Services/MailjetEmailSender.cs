using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionInventaire.BLL.Services
{
    public class MailjetEmailSender : IEmailSender
    {

        private readonly IConfiguration _configuration;

        private readonly ILogger<MailjetEmailSender> _logger;


        public MailjetEmailSender(IConfiguration configuration, ILogger<MailjetEmailSender> logger)

        {

            _configuration = configuration;

            _logger = logger;

        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)

        {

            var apiKey = _configuration["Mailjet:ApiKey"];

            var apiSecret = _configuration["Mailjet:ApiSecret"];

            var senderEmail = _configuration["Mailjet:SenderEmail"];

            var senderName = _configuration["Mailjet:SenderName"];


            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))

            {

                _logger.LogError("Les clés d'API Mailjet sont manquantes dans appsettings.json.");

                return;

            }


            var client = new MailjetClient(apiKey, apiSecret);


            var emailMessage = new TransactionalEmailBuilder()

                .WithFrom(new SendContact(senderEmail, senderName))

                .WithSubject(subject)

                .WithHtmlPart(htmlMessage)

                .WithTo(new SendContact(email))

                .Build();


            var response = await client.SendTransactionalEmailAsync(emailMessage);


            if (response.Messages != null && response.Messages.Length > 0 && response.Messages[0].Status == "success")

            {

                _logger.LogInformation($"Email envoyé avec succès à {email} via Mailjet.");

            }

            else
            {

                _logger.LogError($"Erreur lors de l'envoi de l'email via Mailjet à {email}.");

            }

        }

    }
}
