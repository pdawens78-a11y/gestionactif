using System.Security.Cryptography;
using System.Text;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionInventaire.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<Utilisateur> _userManager;

        private readonly IEmailSender _emailSender;

        public UsersController(
            UserManager<Utilisateur> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;

            _emailSender = emailSender;
        }

        // =========================
        // GET CREATE
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // POST CREATE
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser =
                await _userManager.FindByEmailAsync(
                    model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Un utilisateur avec cet email existe déjà.");

                return View(model);
            }

            // PASSWORD TEMPORAIRE
            var temporaryPassword =
                GenerateTemporaryPassword();

            var user = new Utilisateur
            {
                UserName = model.Email,

                Email = model.Email,

                Nom = model.Nom,

                Prenom = model.Prenom,

                Telephone = model.Telephone,

                EmailConfirmed = false
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    temporaryPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            // ROLE
            await _userManager.AddToRoleAsync(
                user,
                model.Role);

            // TOKEN RESET PASSWORD
            var token =
                await _userManager
                    .GeneratePasswordResetTokenAsync(
                        user);

            token =
                WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(token));

            // URL RESET PASSWORD
            var callbackUrl =
                Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new
                    {
                        area = "Identity",
                        code = token,
                        email = user.Email
                    },
                    protocol: Request.Scheme);

            // EMAIL HTML
            var htmlMessage =
                $@"
                <div style='font-family:Arial;padding:30px;'>

                    <h2>
                        Bienvenue {user.Prenom}
                    </h2>

                    <p>
                        Votre compte TechnoLogis
                        a été créé avec succès.
                    </p>

                    <p>
                        Cliquez sur le bouton ci-dessous
                        pour définir votre mot de passe.
                    </p>

                    <a href='{callbackUrl}'
                       style='
                            display:inline-block;
                            padding:14px 22px;
                            background:#00d2ff;
                            color:white;
                            text-decoration:none;
                            border-radius:10px;
                            margin-top:15px;
                       '>

                        Définir mon mot de passe

                    </a>

                </div>";

            // SEND EMAIL
            await _emailSender.SendEmailAsync(
                user.Email,
                "Bienvenue sur TechnoLogis",
                htmlMessage);

            // REDIRECTION
            return RedirectToPage(
                "/Account/UserInvitationSent",
                new
                {
                    area = "Identity",
                    email = user.Email
                });
        }

        // =========================
        // GENERATE PASSWORD
        // =========================

        private static string GenerateTemporaryPassword()
        {
            return Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(12))
                + "Aa1!";
        }
    }
}