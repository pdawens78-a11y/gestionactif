using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Web.Models.Services
{
    // ── Liste ──
    public class ServiceIndexViewModel
    {
        public List<ServiceRowViewModel> Services { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class ServiceRowViewModel
    {
        public int IdService { get; set; }
        public string NomService { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NombreEmployes { get; set; }

        public string DescriptionFormatted =>
            string.IsNullOrEmpty(Description) ? "—" : Description;
    }

    // ── Création ──
    public class ServiceCreateViewModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom du service")]
        public string NomService { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Maximum 300 caractères")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }

    // ── Modification ──
    public class ServiceEditViewModel
    {
        public int IdService { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom du service")]
        public string NomService { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Maximum 300 caractères")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }

    // ── Suppression ──
    public class ServiceDeleteViewModel
    {
        public int IdService { get; set; }
        public string NomService { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NombreEmployes { get; set; }
    }
}