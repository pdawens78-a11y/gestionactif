using System.ComponentModel.DataAnnotations;
using GestionInventaire.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionInventaire.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;

        public IndexModel(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // =====================================================
        // STATUS MESSAGE
        // =====================================================

        [TempData]
        public string? StatusMessage { get; set; }

        // =====================================================
        // DISPLAY DATA
        // =====================================================

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string NomComplet { get; set; } = string.Empty;

        public string TypeCompte { get; set; } = string.Empty;

        public string Initiales { get; set; } = string.Empty;

        // =====================================================
        // INPUT
        // =====================================================

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Phone(ErrorMessage = "Numéro de téléphone invalide")]
            [Display(Name = "Téléphone")]
            public string? Telephone { get; set; }
        }

        // =====================================================
        // LOAD USER
        // =====================================================

        private async Task LoadAsync(Utilisateur user)
        {
            Username = user.UserName ?? "";

            Email = user.Email ?? "";

            NomComplet = $"{user.Prenom} {user.Nom}";

            Input = new InputModel
            {
                Telephone = user.Telephone
            };

            var roles = await _userManager.GetRolesAsync(user);

            TypeCompte =
                roles.FirstOrDefault() ?? "Utilisateur";

            var prenomInitial =
                !string.IsNullOrWhiteSpace(user.Prenom)
                    ? user.Prenom[0].ToString()
                    : "";

            var nomInitial =
                !string.IsNullOrWhiteSpace(user.Nom)
                    ? user.Nom[0].ToString()
                    : "";

            Initiales =
                $"{prenomInitial}{nomInitial}".ToUpper();
        }

        // =====================================================
        // GET
        // =====================================================

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            var user =
                await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            await LoadAsync(user);

            return Page();
        }

        // =====================================================
        // POST
        // =====================================================

        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            var user =
                await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);

                return Page();
            }

            // UPDATE PHONE
            if (Input.Telephone != user.Telephone)
            {
                user.Telephone = Input.Telephone;

                var result =
                    await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    StatusMessage =
                        "Erreur lors de la mise à jour.";

                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);

            StatusMessage =
                "Votre profil a été mis à jour.";

            return RedirectToPage();
        }
    }
}