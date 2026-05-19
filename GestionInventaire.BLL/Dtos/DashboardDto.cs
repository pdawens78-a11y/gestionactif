namespace GestionInventaire.BLL.Dtos
{
    public class DashboardDto
    {
        public int TotalActifs { get; set; }
        public int ActifsDisponibles { get; set; }
        public int ActifsAffectes { get; set; }
        public int ActifsEnMaintenance { get; set; }
        public int ActifsHorsService { get; set; }
        public int StockCritique { get; set; }
        public List<AlerteStockDto> AlertesStock { get; set; } = new();
        public List<AlerteMaintenanceDto> AlertesMaintenance { get; set; } = new();
        public List<AuditItemDto> DerniersAudits { get; set; } = new();
    }

    public class AlerteStockDto
    {
        public string NomProduit { get; set; } = string.Empty;
        public int Quantite { get; set; }
    }

    public class AlerteMaintenanceDto
    {
        public string NomActif { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public bool EstUrgente { get; set; }
    }

    public class AuditItemDto
    {
        public string Action { get; set; } = string.Empty;
        public string UtilisateurNom { get; set; } = string.Empty;
        public DateTime DateAction { get; set; }
    }
}