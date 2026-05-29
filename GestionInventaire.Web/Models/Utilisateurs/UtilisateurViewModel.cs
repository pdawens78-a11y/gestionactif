using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Models.Utilisateurs
{
    // ── Liste ──
    public class UtilisateurIndexViewModel
    {
        public List<UtilisateurRowViewModel> Utilisateurs { get; set; } = new();
        public int TotalCount { get; set; }
        public int TotalAdmin { get; set; }
        public int TotalGestionnaire { get; set; }
        public int TotalTechnicien { get; set; }
        public int TotalVerrouilles { get; set; }
    }

    public class UtilisateurRowViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string NomComplet { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool EstVerrouille { get; set; }
        public bool EmailConfirme { get; set; }

        public string InitialesAvatar =>
            $"{(Prenom.Length > 0 ? Prenom[0] : ' ')}{(Nom.Length > 0 ? Nom[0] : ' ')}".ToUpper();

        public string RoleClass =>
            Role == "Admin" ? "role-admin"
            : Role == "Gestionnaire" ? "role-gestionnaire"
            : Role == "Technicien" ? "role-technicien"
            : "role-default";

        public string StatutLabel =>
            EstVerrouille ? "Verrouillé" : EmailConfirme ? "Actif" : "Non confirmé";

        public string StatutClass =>
            EstVerrouille ? "statut-verrouille"
            : EmailConfirme ? "statut-actif"
            : "statut-nonconfirme";
    }

    // ── Modification ──
    public class UtilisateurEditViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        [StringLength(20)]
        [Display(Name = "Téléphone")]
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "Le rôle est obligatoire")]
        [Display(Name = "Rôle")]
        public string Role { get; set; } = string.Empty;

        public List<SelectListItem> Roles { get; set; } = new();
    }

    // ── Suppression ──
    public class UtilisateurDeleteViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string NomComplet { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}