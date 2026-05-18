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
        public string? Telephone { get; set; }   // ← Telephone (pas Tel)
        public string NomService { get; set; } = string.Empty; // ← NomService (pas Service)
        public int NombreAffectations { get; set; }
        public int ActifsActifs { get; set; }

        public string EmailFormatted => string.IsNullOrEmpty(Email) ? "—" : Email;
        public string TelephoneFormatted => string.IsNullOrEmpty(Telephone) ? "—" : Telephone;
        public string ServiceFormatted => string.IsNullOrEmpty(NomService) ? "—" : NomService;

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
        public string? Telephone { get; set; }   // ← Telephone (pas Tel)

        [Display(Name = "Service / Département")]
        public int? IdService { get; set; }

        public List<ServiceDropdownItem> Services { get; set; } = new();
    }

    public class ServiceDropdownItem
    {
        public int IdService { get; set; }
        public string NomService { get; set; } = string.Empty;
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
        public string? Telephone { get; set; }   // ← Telephone (pas Tel)

        [Display(Name = "Service / Département")]
        public int? IdService { get; set; }

        public List<ServiceDropdownItem> Services { get; set; } = new();

        public int NombreAffectations { get; set; }
        public int ActifsActifs { get; set; }
    }

    // ── Suppression ──
    public class EmployeDeleteViewModel
    {
        public int IdEmploye { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string NomService { get; set; } = string.Empty;
        public int NombreAffectations { get; set; }
        public int ActifsActifs { get; set; }
    }
}