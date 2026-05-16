using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Models.Actifs
{
    // ── Liste ──
    public class ActifIndexViewModel
    {
        public List<ActifRowViewModel> Actifs { get; set; } = new();
        public int TotalCount { get; set; }
        public int TotalDisponibles { get; set; }
        public int TotalAffectes { get; set; }
        public int TotalEnMaintenance { get; set; }
        public int TotalHorsService { get; set; }

        // Filtre actif
        public string? FiltreStatut { get; set; }
    }

    public class ActifRowViewModel
    {
        public int IdActif { get; set; }
        public string CodeInventaire { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;
        public string Localisation { get; set; } = string.Empty;
        public string Statut { get; set; } = string.Empty;
        public DateTime DateAcquisition { get; set; }

        public string DateFormatted => DateAcquisition.ToString("dd/MM/yyyy");

        public string StatutLabel =>
            Statut == "Disponible" ? "Disponible"
            : Statut == "Affecte" ? "Affecté"
            : Statut == "EnMaintenance" ? "En maintenance"
            : "Hors service";

        public string StatutClass =>
            Statut == "Disponible" ? "statut-disponible"
            : Statut == "Affecte" ? "statut-affecte"
            : Statut == "EnMaintenance" ? "statut-maintenance"
            : "statut-hors-service";
    }

    // ── Modification ──
    public class ActifEditViewModel
    {
        public int IdActif { get; set; }
        public string CodeInventaire { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;

        [Required(ErrorMessage = "La localisation est obligatoire")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "La localisation doit contenir entre 3 et 100 caractères")]
        [Display(Name = "Localisation")]
        public string Localisation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le statut est obligatoire")]
        [Display(Name = "Statut")]
        public string Statut { get; set; } = string.Empty;

        // Seuls Disponible et HorsService sont modifiables manuellement
        public List<SelectListItem> Statuts { get; set; } = new()
        {
            new SelectListItem("Disponible",    "Disponible"),
            new SelectListItem("Hors service",  "HorsService")
        };
    }

    // ── Approvisionnement ──
    public class ApprovisionnerViewModel
    {
        public int IdProduit { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;
        public int StockActuel { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(1, 10000, ErrorMessage = "La quantité doit être entre 1 et 10 000")]
        [Display(Name = "Nombre d'actifs à générer")]
        public int Quantite { get; set; } = 1;

        [Required(ErrorMessage = "La localisation est obligatoire")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "La localisation doit contenir entre 3 et 100 caractères")]
        [Display(Name = "Localisation")]
        public string Localisation { get; set; } = string.Empty;
    }

    // ── Résultat approvisionnement ──
    public class ApprovisionnerResultViewModel
    {
        public int NombreGenere { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public List<string> CodesGeneres { get; set; } = new();

        // Afficher les 10 premiers + "et X autres"
        public List<string> CodesAffiches =>
            CodesGeneres.Take(10).ToList();

        public int CodesRestants =>
            Math.Max(0, CodesGeneres.Count - 10);
    }
}