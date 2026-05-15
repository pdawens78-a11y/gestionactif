using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GestionInventaire.Web.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage)
        {
            var apiKey =
                _configuration["Email:Mailjet:ApiKey"];

            var apiSecret =
                _configuration["Email:Mailjet:Secret"];

            var senderEmail =
                _configuration["Email:Mailjet:SenderEmail"];

            var senderName =
                _configuration["Email:Mailjet:SenderName"];

            var client =
                new MailjetClient(
                    apiKey,
                    apiSecret);

            var message =
                new TransactionalEmailBuilder()

                .WithFrom(
                    new SendContact(
                        senderEmail,
                        senderName))

                .WithSubject(subject)

                .WithHtmlPart(htmlMessage)

                .WithTo(
                    new SendContact(email))

                .Build();

            await client.SendTransactionalEmailAsync(
                message);
        }
    }
}