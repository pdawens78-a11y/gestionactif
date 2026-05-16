using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Web.Models.Categories
{
    // ── Liste ──
    public class CategorieIndexViewModel
    {
        public List<CategorieRowViewModel> Categories { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class CategorieRowViewModel
    {
        public int IdCategorie { get; set; }
        public string NomCategorie { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NombreProduits { get; set; }

        public string DescriptionFormatted =>
            string.IsNullOrEmpty(Description) ? "—" : Description;
    }

    // ── Création ──
    public class CategorieCreateViewModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom de la catégorie")]
        public string NomCategorie { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Maximum 500 caractères")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }

    // ── Modification ──
    public class CategorieEditViewModel
    {
        public int IdCategorie { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom de la catégorie")]
        public string NomCategorie { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Maximum 500 caractères")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public int NombreProduits { get; set; }
    }

    // ── Suppression ──
    public class CategorieDeleteViewModel
    {
        public int IdCategorie { get; set; }
        public string NomCategorie { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NombreProduits { get; set; }
    }
}