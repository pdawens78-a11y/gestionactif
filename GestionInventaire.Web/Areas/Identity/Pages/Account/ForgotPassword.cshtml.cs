// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GestionInventaire.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionInventaire.Web.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<Utilisateur> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
            Input = new InputModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
    Input.Email,
    "Réinitialisation du mot de passe - TechnoLogis",
    $@"
    <div style='
        font-family:Arial,sans-serif;
        background:#0f172a;
        padding:40px;
        color:white;
    '>

        <div style='
            max-width:600px;
            margin:auto;
            background:#111827;
            border-radius:20px;
            padding:40px;
            border:1px solid rgba(255,255,255,.06);
        '>

            <h1 style='
                margin-top:0;
                color:#00d4aa;
            '>
                TechnoLogis
            </h1>

            <h2 style='
                color:white;
                margin-bottom:20px;
            '>
                Réinitialisation du mot de passe
            </h2>

            <p style='
                color:#cbd5e1;
                line-height:1.8;
            '>
                Bonjour,
            </p>

            <p style='
                color:#cbd5e1;
                line-height:1.8;
            '>
                Une demande de réinitialisation de mot de passe
                a été effectuée pour votre compte.
            </p>

            <p style='
                color:#cbd5e1;
                line-height:1.8;
            '>
                Cliquez sur le bouton ci-dessous pour définir
                un nouveau mot de passe sécurisé.
            </p>

            <div style='margin:40px 0;'>

                <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'
                   style='
                        background:linear-gradient(135deg,#00d4aa,#3b82f6);
                        color:white;
                        padding:16px 28px;
                        border-radius:12px;
                        text-decoration:none;
                        font-weight:600;
                        display:inline-block;
                   '>

                    Réinitialiser le mot de passe

                </a>

            </div>

            <p style='
                color:#94a3b8;
                font-size:14px;
                line-height:1.7;
            '>
                Si vous n'êtes pas à l'origine de cette demande,
                vous pouvez ignorer cet e-mail en toute sécurité.
            </p>

        </div>

    </div>");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}