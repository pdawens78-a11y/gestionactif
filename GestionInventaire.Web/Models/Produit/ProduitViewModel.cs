using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Models.Produits
{
    // ── Liste ──
    public class ProduitIndexViewModel
    {
        public List<ProduitRowViewModel> Produits { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class ProduitRowViewModel
    {
        public int IdProduit { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string NomCategorie { get; set; } = string.Empty;
        public int NombreActifs { get; set; }
        public int? StockQuantite { get; set; }
        public int? StockSeuil { get; set; }
        public bool StockCritique { get; set; }

        public string DescriptionFormatted =>
            string.IsNullOrEmpty(Description) ? "—" : Description;

        public string StockFormatted =>
            StockQuantite.HasValue ? StockQuantite.Value.ToString() : "—";

        public string StockBadgeClass =>
            !StockQuantite.HasValue ? "stock-none"
            : StockCritique ? "stock-critical"
            : "stock-ok";
    }

    // ── Création avec approvisionnement ──
    public class ProduitCreateViewModel
    {
        // ── Infos produit ──
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom du produit")]
        public string NomProduit { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Maximum 500 caractères")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        [Display(Name = "Catégorie")]
        public int IdCategorie { get; set; }

        // ── Stock ──
        [Range(0, int.MaxValue, ErrorMessage = "Le seuil ne peut pas être négatif")]
        [Display(Name = "Seuil d'alerte stock")]
        public int SeuilAlerte { get; set; } = 5;

        // ── Actifs ──
        [Range(0, 10000,
            ErrorMessage = "La quantité doit être entre 0 et 10 000")]
        [Display(Name = "Nombre d'actifs à générer")]
        public int QuantiteActifs { get; set; } = 0;

        [StringLength(100)]
        [Display(Name = "Localisation des actifs")]
        public string Localisation { get; set; } = string.Empty;

        // Listes déroulantes
        public List<SelectListItem> Categories { get; set; } = new();
    }

    // ── Résultat création ──
    public class ProduitCreateResultViewModel
    {
        public string NomProduit { get; set; } = string.Empty;
        public int NombreActifs { get; set; }
        public List<string> CodesGeneres { get; set; } = new();

        public List<string> CodesAffiches =>
            CodesGeneres.Take(10).ToList();

        public int CodesRestants =>
            Math.Max(0, CodesGeneres.Count - 10);
    }

    // ── Modification ──
    public class ProduitEditViewModel
    {
        public int IdProduit { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom du produit")]
        public string NomProduit { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Maximum 500 caractères")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        [Display(Name = "Catégorie")]
        public int IdCategorie { get; set; }

        public int NombreActifs { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }

    // ── Suppression ──
    public class ProduitDeleteViewModel
    {
        public int IdProduit { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string NomCategorie { get; set; } = string.Empty;
        public int NombreActifs { get; set; }
    }
}
