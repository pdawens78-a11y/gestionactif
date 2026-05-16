using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Models.Maintenances
{
    // ── Liste ──
    public class MaintenanceIndexViewModel
    {
        public List<MaintenanceRowViewModel> Maintenances { get; set; } = new();
        public int TotalCount { get; set; }
        public int Planifiees { get; set; }
        public int EnCours { get; set; }
        public int Terminees { get; set; }
    }

    public class MaintenanceRowViewModel
    {
        public int IdMaintenance { get; set; }
        public string CodeActif { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cout { get; set; }
        public string Statut { get; set; } = string.Empty;
        public bool EstUrgente { get; set; }

        public string DateFormatted => DateMaintenance.ToString("dd/MM/yyyy");
        public string CoutFormatted => $"{Cout:N2} HTG";

        public string StatutLabel =>
            Statut == "Planifiee" ? "Planifiée"
            : Statut == "EnCours" ? "En cours"
            : "Terminée";

        public string StatutClass =>
            Statut == "Planifiee" ? "statut-planifiee"
            : Statut == "EnCours" ? "statut-encours"
            : "statut-terminee";
    }

    // ── Création ──
    public class MaintenanceCreateViewModel
    {
        [Required(ErrorMessage = "L'actif est obligatoire")]
        [Display(Name = "Actif")]
        public int IdActif { get; set; }

        [Required(ErrorMessage = "La date est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de maintenance")]
        public DateTime DateMaintenance { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(500, ErrorMessage = "Maximum 500 caractères")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Le coût ne peut pas être négatif")]
        [Display(Name = "Coût estimé (HTG)")]
        public decimal Cout { get; set; }

        public List<SelectListItem> Actifs { get; set; } = new();
    }

    // ── Modification ──
    public class MaintenanceEditViewModel
    {
        public int IdMaintenance { get; set; }
        public string CodeActif { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de maintenance")]
        public DateTime DateMaintenance { get; set; }

        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Le coût ne peut pas être négatif")]
        [Display(Name = "Coût (HTG)")]
        public decimal Cout { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire")]
        [Display(Name = "Statut")]
        public string Statut { get; set; } = string.Empty;

        public List<SelectListItem> Statuts { get; set; } = new()
        {
            new SelectListItem("Planifiée",  "Planifiee"),
            new SelectListItem("En cours",   "EnCours"),
            new SelectListItem("Terminée",   "Terminee")
        };
    }

    // ── Suppression ──
    public class MaintenanceDeleteViewModel
    {
        public int IdMaintenance { get; set; }
        public string CodeActif { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Statut { get; set; } = string.Empty;

        public string DateFormatted => DateMaintenance.ToString("dd/MM/yyyy");
        public string StatutLabel =>
            Statut == "Planifiee" ? "Planifiée"
            : Statut == "EnCours" ? "En cours"
            : "Terminée";
    }
}