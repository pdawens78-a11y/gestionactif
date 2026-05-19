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

    // ── Modification actif ──
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

        public List<SelectListItem> Statuts { get; set; } = new()
        {
            new SelectListItem("Disponible",   "Disponible"),
            new SelectListItem("Hors service", "HorsService")
        };
    }

    // ── Approvisionnement ──
    public class ApprovisionnerViewModel
    {
        public int IdProduit { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;

        // ── Infos contextuelles ──
        public int StockActuel { get; set; }   // quantité cible dans le stock
        public int NombreActifs { get; set; }   // actifs déjà créés
        public bool QuantiteAuto { get; set; }   // true = quantité calculée automatiquement

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(1, 10000, ErrorMessage = "La quantité doit être entre 1 et 10 000")]
        [Display(Name = "Nombre d'actifs à générer")]
        public int Quantite { get; set; } = 1;

        [Required(ErrorMessage = "La localisation est obligatoire")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "La localisation doit contenir entre 3 et 100 caractères")]
        [Display(Name = "Localisation")]
        public string Localisation { get; set; } = string.Empty;

        // ── Calculés ──
        public int ActifsManquants => Math.Max(0, StockActuel - NombreActifs);
        public bool StockCoherent => NombreActifs >= StockActuel;
    }

    // ── Résultat ──
    public class ApprovisionnerResultViewModel
    {
        public int NombreGenere { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public List<string> CodesGeneres { get; set; } = new();

        public List<string> CodesAffiches => CodesGeneres.Take(10).ToList();
        public int CodesRestants => Math.Max(0, CodesGeneres.Count - 10);
    }
}