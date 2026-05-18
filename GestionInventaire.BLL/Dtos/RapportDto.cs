namespace GestionInventaire.BLL.Dtos
{
    public class RapportDto
    {
        public DateTime              DateGeneration { get; set; }
        public RapportInventaireDto  Inventaire     { get; set; } = new();
        public RapportStockDto       Stock          { get; set; } = new();
        public RapportAffectationDto Affectations   { get; set; } = new();
        public RapportMaintenanceDto Maintenances   { get; set; } = new();
    }

    // ── Section 1 : Inventaire ──
    public class RapportInventaireDto
    {
        public int                        TotalActifs        { get; set; }
        public int                        TotalDisponibles   { get; set; }
        public int                        TotalAffectes      { get; set; }
        public int                        TotalEnMaintenance { get; set; }
        public int                        TotalHorsService   { get; set; }
        public List<RapportActifLigneDto> Actifs             { get; set; } = new();
    }

    public class RapportActifLigneDto
    {
        public string   CodeInventaire  { get; set; } = string.Empty;
        public string   NomProduit      { get; set; } = string.Empty;
        public string   NomCategorie    { get; set; } = string.Empty;
        public string   Localisation    { get; set; } = string.Empty;
        public string   Statut          { get; set; } = string.Empty;
        public DateTime DateAcquisition { get; set; }
    }

    // ── Section 2 : Stock ──
    public class RapportStockDto
    {
        public int                       TotalProduits   { get; set; }
        public int                       StocksCritiques { get; set; }
        public int                       StocksEpuises   { get; set; }
        public List<RapportStockLigneDto> Stocks         { get; set; } = new();
    }

    public class RapportStockLigneDto
    {
        public string NomProduit   { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;
        public int    Quantite     { get; set; }
        public int    SeuilAlerte  { get; set; }
        public bool   EstCritique  { get; set; }
        public bool   EstEpuise    { get; set; }
    }

    // ── Section 3 : Affectations ──
    public class RapportAffectationDto
    {
        public int                              TotalActives { get; set; }
        public List<RapportAffectationLigneDto> Affectations { get; set; } = new();
    }

    public class RapportAffectationLigneDto
    {
        public string   CodeActif  { get; set; } = string.Empty;
        public string   NomProduit { get; set; } = string.Empty;
        public string   NomEmploye { get; set; } = string.Empty;
        public string   Service    { get; set; } = string.Empty;
        public DateTime DateDebut  { get; set; }
        public int      DureeJours { get; set; }
    }

    // ── Section 4 : Maintenances ──
    public class RapportMaintenanceDto
    {
        public int                               TotalMaintenances { get; set; }
        public int                               EnCours           { get; set; }
        public decimal                           CoutTotal         { get; set; }
        public List<RapportMaintenanceLigneDto>  Maintenances      { get; set; } = new();
    }

    public class RapportMaintenanceLigneDto
    {
        public string   CodeActif       { get; set; } = string.Empty;
        public string   NomProduit      { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public string   Statut          { get; set; } = string.Empty;
        public decimal  Cout            { get; set; }
        public string   Description     { get; set; } = string.Empty;
    }
}
