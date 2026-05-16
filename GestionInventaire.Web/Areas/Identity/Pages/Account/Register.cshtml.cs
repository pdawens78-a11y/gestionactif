#nullable disable

using System.ComponentModel.DataAnnotations;
using GestionInventaire.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionInventaire.Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;

        private readonly SignInManager<Utilisateur> _signInManager;

        public RegisterModel(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager)
        {
            _userManager = userManager;

            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Adresse e-mail")]
            public string Email { get; set; }

            [Required]
            [StringLength(
                100,
                ErrorMessage =
                    "Le mot de passe doit contenir au moins {2} caractères.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmation")]
            [Compare(
                "Password",
                ErrorMessage =
                    "Les mots de passe ne correspondent pas.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync()
        {
            ExternalLogins =
                (await _signInManager
                    .GetExternalAuthenticationSchemesAsync())
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ExternalLogins =
                (await _signInManager
                    .GetExternalAuthenticationSchemesAsync())
                .ToList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser =
                await _userManager.FindByEmailAsync(
                    Input.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Un compte avec cet email existe déjà.");

                return Page();
            }

            var user = new Utilisateur
            {
                UserName = Input.Email,

                Email = Input.Email,

                EmailConfirmed = true
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    Input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return Page();
            }

            return RedirectToPage(
                "/Account/Login");
        }
    }
}