using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Models.Affectations
{
    // ── Vue liste ──
    public class AffectationIndexViewModel
    {
        public List<AffectationRowViewModel> Affectations { get; set; } = new();
        public int TotalActives { get; set; }
        public int TotalTerminees { get; set; }
    }

    public class AffectationRowViewModel
    {
        public int IdAffectation { get; set; }
        public string CodeActif { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;
        public string NomEmploye { get; set; } = string.Empty;
        public string ServiceEmploye { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool EstActive { get; set; }

        public string DateDebutFormatted => DateDebut.ToString("dd/MM/yyyy");
        public string DateFinFormatted => DateFin?.ToString("dd/MM/yyyy") ?? "En cours";

        public string BadgeClass => EstActive ? "badge-active" : "badge-terminee";
        public string BadgeLabel => EstActive ? "Active" : "Terminée";
    }

    // ── Vue formulaire création ──
    public class AffectationCreateViewModel
    {
        [Required(ErrorMessage = "L'actif est obligatoire")]
        [Display(Name = "Actif à affecter")]
        public int IdActif { get; set; }

        [Required(ErrorMessage = "L'employé est obligatoire")]
        [Display(Name = "Employé bénéficiaire")]
        public int IdEmploye { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire")]
        [Display(Name = "Date de début")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; } = DateTime.Today;

        // Listes déroulantes
        public List<SelectListItem> ActifsDisponibles { get; set; } = new();
        public List<SelectListItem> Employes { get; set; } = new();
    }
}