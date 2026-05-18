namespace GestionInventaire.BLL.Dtos
{
    public class MaintenanceListDto
    {
        public List<MaintenanceItemDto> Maintenances { get; set; } = new();
        public int                      TotalCount   { get; set; }
        public int                      Planifiees   { get; set; }
        public int                      EnCours      { get; set; }
        public int                      Terminees    { get; set; }
    }

    public class MaintenanceItemDto
    {
        public int      IdMaintenance   { get; set; }
        public string   CodeActif       { get; set; } = string.Empty;
        public string   NomProduit      { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public string   Description     { get; set; } = string.Empty;
        public decimal  Cout            { get; set; }
        public string   Statut          { get; set; } = string.Empty;
        public bool     EstUrgente      { get; set; }
    }

    public class MaintenanceDetailDto
    {
        public int      IdMaintenance   { get; set; }
        public int      IdActif         { get; set; }
        public string   CodeActif       { get; set; } = string.Empty;
        public string   NomProduit      { get; set; } = string.Empty;
        public DateTime DateMaintenance { get; set; }
        public string   Description     { get; set; } = string.Empty;
        public decimal  Cout            { get; set; }
        public string   Statut          { get; set; } = string.Empty;
    }

    public class MaintenanceCreateDto
    {
        public int      IdActif         { get; set; }
        public DateTime DateMaintenance { get; set; }
        public string   Description     { get; set; } = string.Empty;
        public decimal  Cout            { get; set; }
    }

    public class MaintenanceEditDto
    {
        public int      IdMaintenance   { get; set; }
        public DateTime DateMaintenance { get; set; }
        public string   Description     { get; set; } = string.Empty;
        public decimal  Cout            { get; set; }
        public string   Statut          { get; set; } = string.Empty;
    }

    public class ActifDisponibleMaintenanceDto
    {
        public int    IdActif        { get; set; }
        public string CodeInventaire { get; set; } = string.Empty;
        public string NomProduit     { get; set; } = string.Empty;
        public string Statut         { get; set; } = string.Empty;
    }
}
