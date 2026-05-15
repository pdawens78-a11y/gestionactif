using System;
using System.Collections.Generic;

namespace GestionInventaire.Web.Models.Dashboard
{
    public class DashboardViewModel
    {
        // Indicateurs principaux
        public int TotalActifs { get; set; }
        public int ActifsDisponibles { get; set; }
        public int ActifsAffectes { get; set; }
        public int ActifsEnMaintenance { get; set; }
        public int ActifsHorsService { get; set; }
        public int StockCritique { get; set; }

        // Indicateurs calculés
        public double TauxDisponibilite =>
            TotalActifs == 0 ? 0 :
            (double)ActifsDisponibles / TotalActifs * 100;

        public string TauxDisponibiliteFormatted =>
            $"{TauxDisponibilite:0.##}%";

        public double TauxEnMaintenance =>
            TotalActifs == 0 ? 0 :
            (double)ActifsEnMaintenance / TotalActifs * 100;

        public string TauxEnMaintenanceFormatted =>
            $"{TauxEnMaintenance:0.##}%";

        public double TauxDAffectation =>
            TotalActifs == 0 ? 0 :
            (double)ActifsAffectes / TotalActifs * 100;

        public string TauxDAffectationFormatted =>
            $"{TauxDAffectation:0.##}%";

        // Alertes
        public List<AlerteStockViewModel> AlertesStock { get; set; } = new();
        public List<AlerteMaintenanceViewModel> AlertesMaintenance { get; set; } = new();
        public List<AuditItemViewModel> DerniersAudits { get; set; } = new();
    }

    public class AlerteStockViewModel
    {
        public string NomActif { get; set; } = string.Empty;
        public int Quantite { get; set; }

        public bool EstCritique => Quantite <= 1;

        public string BadgeClass =>
            EstCritique ? "badge-danger" : "badge-warning";
    }

    public class AlerteMaintenanceViewModel
    {
        public string NomActif { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }

        public string DateFormatted =>
            DateMaintenance.ToString("dd/MM/yyyy");

        public bool EstUrgente =>
            DateMaintenance <= DateTime.Now.AddDays(3);

        public string BadgeClass =>
            EstUrgente ? "badge-danger" : "badge-warning";
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