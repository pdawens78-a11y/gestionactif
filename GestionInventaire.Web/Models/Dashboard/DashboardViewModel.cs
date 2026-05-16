namespace GestionInventaire.Web.Models.Dashboard
{
    public class DashboardViewModel
    {
        // ── Stats actifs ──
        public int TotalActifs { get; set; }
        public int ActifsDisponibles { get; set; }
        public int ActifsAffectes { get; set; }
        public int ActifsEnMaintenance { get; set; }
        public int ActifsHorsService { get; set; }
        public int StockCritique { get; set; }

        // ── Propriétés calculées pour la vue ──
        public string TauxDisponibiliteFormatted =>
            TotalActifs > 0
                ? $"{(ActifsDisponibles * 100.0 / TotalActifs):F0}%"
                : "0%";

        public string TauxDAffectationFormatted =>
            TotalActifs > 0
                ? $"{(ActifsAffectes * 100.0 / TotalActifs):F0}%"
                : "0%";

        public string TauxEnMaintenanceFormatted =>
            TotalActifs > 0
                ? $"{(ActifsEnMaintenance * 100.0 / TotalActifs):F0}%"
                : "0%";

        // ── Alertes ──
        public List<AlerteStockViewModel> AlertesStock { get; set; } = new();
        public List<AlerteMaintenanceViewModel> AlertesMaintenance { get; set; } = new();

        // ── Audits ──
        public List<AuditItemViewModel> DerniersAudits { get; set; } = new();
    }

    public class AlerteStockViewModel
    {
        public string NomProduit { get; set; } = string.Empty;
        public int Quantite { get; set; }
    }

    public class AlerteMaintenanceViewModel
    {
        public string NomActif { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public bool EstUrgente { get; set; }

        public string DateFormatted =>
            DateMaintenance.ToString("dd/MM/yyyy");
    }

    public class AuditItemViewModel
    {
        public string Action { get; set; } = string.Empty;
        public string UtilisateurNom { get; set; } = string.Empty;
        public DateTime DateAction { get; set; }

        public string DateFormatted =>
            DateAction.ToString("dd/MM/yyyy HH:mm");
    }
}