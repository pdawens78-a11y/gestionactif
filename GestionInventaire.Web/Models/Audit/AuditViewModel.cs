using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Web.Models.Audit
{
    public class AuditIndexViewModel
    {
        public List<AuditRowViewModel> Logs { get; set; } = new();
        public int TotalCount { get; set; }
        public AuditFiltreViewModel Filtre { get; set; } = new();
    }

    public class AuditRowViewModel
    {
        public int IdAuditLog { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Entite { get; set; } = string.Empty;
        public int EntiteId { get; set; }
        public DateTime DateAction { get; set; }
        public string UtilisateurNom { get; set; } = string.Empty;

        public string DateFormatted =>
            DateAction.ToString("dd/MM/yyyy HH:mm");

        public string EntiteClass =>
            Entite.ToLower() switch
            {
                "actif" => "entite-actif",
                "affectation" => "entite-affectation",
                "maintenance" => "entite-maintenance",
                "stock" => "entite-stock",
                "categorie" => "entite-categorie",
                "produit" => "entite-produit",
                "employe" => "entite-employe",
                _ => "entite-default"
            };

        public string ActionIcon =>
            Action.ToLower() switch
            {
                var a when a.Contains("création") || a.Contains("créé") => "bi-plus-circle",
                var a when a.Contains("modification") || a.Contains("mis") => "bi-pencil",
                var a when a.Contains("suppression") || a.Contains("supprimé") => "bi-trash",
                var a when a.Contains("affectation") => "bi-person-check",
                var a when a.Contains("retour") => "bi-arrow-return-left",
                var a when a.Contains("maintenance") => "bi-wrench",
                var a when a.Contains("statut") => "bi-flag",
                _ => "bi-activity"
            };
    }

    public class AuditFiltreViewModel
    {
        [Display(Name = "Recherche")]
        public string? Query { get; set; }

        [Display(Name = "Action")]
        public string? Action { get; set; }

        [Display(Name = "Date début")]
        [DataType(DataType.Date)]
        public DateTime? DateDebut { get; set; }

        [Display(Name = "Date fin")]
        [DataType(DataType.Date)]
        public DateTime? DateFin { get; set; }
    }
}