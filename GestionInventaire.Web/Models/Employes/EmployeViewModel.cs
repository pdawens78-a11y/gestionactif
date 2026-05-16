using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Web.Models.Employes
{
    // ── Liste ──
    public class EmployeIndexViewModel
    {
        public List<EmployeRowViewModel> Employes { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class EmployeRowViewModel
    {
        public int IdEmploye { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Tel { get; set; }
        public string? Service { get; set; }
        public int NombreAffectations { get; set; }
        public int ActifsActifs { get; set; }

        public string EmailFormatted => string.IsNullOrEmpty(Email) ? "—" : Email;
        public string TelFormatted => string.IsNullOrEmpty(Tel) ? "—" : Tel;
        public string ServiceFormatted => string.IsNullOrEmpty(Service) ? "—" : Service;

        public string InitialesAvatar =>
            $"{(Prenom.Length > 0 ? Prenom[0] : ' ')}{(Nom.Length > 0 ? Nom[0] : ' ')}".ToUpper();
    }

    // ── Création ──
    public class EmployeCreateViewModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        [StringLength(150)]
        [Display(Name = "Adresse e-mail")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        [StringLength(20)]
        [Display(Name = "Téléphone")]
        public string? Tel { get; set; }

        [StringLength(100)]
        [Display(Name = "Service / Département")]
        public string? Service { get; set; }
    }

    // ── Modification ──
    public class EmployeEditViewModel
    {
        public int IdEmploye { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        [StringLength(150)]
        [Display(Name = "Adresse e-mail")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        [StringLength(20)]
        [Display(Name = "Téléphone")]
        public string? Tel { get; set; }

        [StringLength(100)]
        [Display(Name = "Service / Département")]
        public string? Service { get; set; }

        public int NombreAffectations { get; set; }
        public int ActifsActifs { get; set; }
    }

    // ── Suppression ──
    public class EmployeDeleteViewModel
    {
        public int IdEmploye { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string? Service { get; set; }
        public int NombreAffectations { get; set; }
        public int ActifsActifs { get; set; }
    }
}