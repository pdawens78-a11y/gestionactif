namespace GestionInventaire.BLL.Dtos
{
    public class ActifListDto
    {
        public List<ActifItemDto> Actifs             { get; set; } = new();
        public int                TotalCount         { get; set; }
        public int                TotalDisponibles   { get; set; }
        public int                TotalAffectes      { get; set; }
        public int                TotalEnMaintenance { get; set; }
        public int                TotalHorsService   { get; set; }
    }

    public class ActifItemDto
    {
        public int      IdActif         { get; set; }
        public string   CodeInventaire  { get; set; } = string.Empty;
        public string   NomProduit      { get; set; } = string.Empty;
        public string   NomCategorie    { get; set; } = string.Empty;
        public string   Localisation    { get; set; } = string.Empty;
        public string   Statut          { get; set; } = string.Empty;
        public DateTime DateAcquisition { get; set; }
    }

    public class ActifEditDto
    {
        public int    IdActif        { get; set; }
        public string CodeInventaire { get; set; } = string.Empty;
        public string NomProduit     { get; set; } = string.Empty;
        public string Localisation   { get; set; } = string.Empty;
        public string Statut         { get; set; } = string.Empty;
    }

    public class ActifUpdateDto
    {
        public int    IdActif      { get; set; }
        public string Localisation { get; set; } = string.Empty;
        public string Statut       { get; set; } = string.Empty;
    }

    public class ApprovisionnerDto
    {
        public int    IdProduit    { get; set; }
        public int    Quantite     { get; set; }
        public string Localisation { get; set; } = string.Empty;
    }

    public class ApprovisionnerResultDto
    {
        public int           NombreGenere { get; set; }
        public string        NomProduit   { get; set; } = string.Empty;
        public List<string>  CodesGeneres { get; set; } = new();
    }
}
