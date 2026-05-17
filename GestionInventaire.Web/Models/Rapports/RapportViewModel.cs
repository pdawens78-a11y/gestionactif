namespace GestionInventaire.Web.Models.Rapports
{
    public class RapportIndexViewModel
    {
        public DateTime DateGeneration { get; set; }
        public RapportInventaireViewModel Inventaire { get; set; } = new();
        public RapportStockViewModel Stock { get; set; } = new();
        public RapportAffectationViewModel Affectations { get; set; } = new();
        public RapportMaintenanceViewModel Maintenances { get; set; } = new();

        public string DateGenerationFormatted =>
            DateGeneration.ToString("dd/MM/yyyy à HH:mm");
    }

    // ── Section 1 ──
    public class RapportInventaireViewModel
    {
        public int TotalActifs { get; set; }
        public int TotalDisponibles { get; set; }
        public int TotalAffectes { get; set; }
        public int TotalEnMaintenance { get; set; }
        public int TotalHorsService { get; set; }
        public List<RapportActifLigneViewModel> Actifs { get; set; } = new();
    }

    public class RapportActifLigneViewModel
    {
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

    // ── Section 2 ──
    public class RapportStockViewModel
    {
        public int TotalProduits { get; set; }
        public int StocksCritiques { get; set; }
        public int StocksEpuises { get; set; }
        public List<RapportStockLigneViewModel> Stocks { get; set; } = new();
    }

    public class RapportStockLigneViewModel
    {
        public string NomProduit { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public int SeuilAlerte { get; set; }
        public bool EstCritique { get; set; }
        public bool EstEpuise { get; set; }

        public string StatusClass =>
            EstEpuise ? "stock-epuise"
            : EstCritique ? "stock-critique"
            : "stock-ok";

        public string StatusLabel =>
            EstEpuise ? "Épuisé"
            : EstCritique ? "Critique"
            : "Normal";
    }

    // ── Section 3 ──
    public class RapportAffectationViewModel
    {
        public int TotalActives { get; set; }
        public List<RapportAffectationLigneViewModel> Affectations { get; set; } = new();
    }

    public class RapportAffectationLigneViewModel
    {
        public string CodeActif { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;
        public string NomEmploye { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public int DureeJours { get; set; }

        public string DateDebutFormatted => DateDebut.ToString("dd/MM/yyyy");
        public string DureeLabel =>
            DureeJours == 0 ? "Aujourd'hui"
            : DureeJours == 1 ? "1 jour"
            : $"{DureeJours} jours";
    }

    // ── Section 4 ──
    public class RapportMaintenanceViewModel
    {
        public int TotalMaintenances { get; set; }
        public int EnCours { get; set; }
        public decimal CoutTotal { get; set; }
        public List<RapportMaintenanceLigneViewModel> Maintenances { get; set; } = new();

        public string CoutTotalFormatted => $"{CoutTotal:N2} HTG";
    }

    public class RapportMaintenanceLigneViewModel
    {
        public string CodeActif { get; set; } = string.Empty;
        public string NomProduit { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public string Statut { get; set; } = string.Empty;
        public decimal Cout { get; set; }
        public string Description { get; set; } = string.Empty;

        public string DateFormatted => DateMaintenance.ToString("dd/MM/yyyy");
        public string CoutFormatted => $"{Cout:N2} HTG";

        public string StatutLabel =>
            Statut == "Planifiee" ? "Planifiée"
            : Statut == "EnCours" ? "En cours"
            : "Terminée";

        public string StatutClass =>
            Statut == "Planifiee" ? "statut-planifiee"
            : Statut == "EnCours" ? "statut-encours"
            : "statut-terminee";
    }
}