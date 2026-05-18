namespace GestionInventaire.BLL.Dtos
{
    public class AffectationListDto
    {
        public List<AffectationItemDto> Affectations   { get; set; } = new();
        public int                      TotalActives   { get; set; }
        public int                      TotalTerminees { get; set; }
    }

    public class AffectationItemDto
    {
        public int       IdAffectation  { get; set; }
        public string    CodeActif      { get; set; } = string.Empty;
        public string    NomProduit     { get; set; } = string.Empty;
        public string    NomEmploye     { get; set; } = string.Empty;
        public string    ServiceEmploye { get; set; } = string.Empty;
        public DateTime  DateDebut      { get; set; }
        public DateTime? DateFin        { get; set; }
        public bool      EstActive      { get; set; }
    }

    public class AffectationCreateDto
    {
        public int      IdActif   { get; set; }
        public int      IdEmploye { get; set; }
        public DateTime DateDebut { get; set; }
    }

    public class AffectationFormDataDto
    {
        public List<ActifDisponibleDto> ActifsDisponibles { get; set; } = new();
        public List<EmployeSelectDto>   Employes          { get; set; } = new();
    }

    public class ActifDisponibleDto
    {
        public int    IdActif        { get; set; }
        public string CodeInventaire { get; set; } = string.Empty;
        public string NomProduit     { get; set; } = string.Empty;
        public string Localisation   { get; set; } = string.Empty;
    }

    public class EmployeSelectDto
    {
        public int    IdEmploye  { get; set; }
        public string NomComplet { get; set; } = string.Empty;
        public string Service    { get; set; } = string.Empty;
    }
}
