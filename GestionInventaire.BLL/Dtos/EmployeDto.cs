namespace GestionInventaire.BLL.Dtos
{
    public class EmployeListDto
    {
        public List<EmployeItemDto> Employes   { get; set; } = new();
        public int                  TotalCount { get; set; }
    }

    public class EmployeItemDto
    {
        public int     IdEmploye          { get; set; }
        public string  Nom                { get; set; } = string.Empty;
        public string  Prenom             { get; set; } = string.Empty;
        public string  NomComplet         { get; set; } = string.Empty;
        public string? Email              { get; set; }
        public string? Telephone          { get; set; }
        public int?    IdService          { get; set; }
        public string  NomService         { get; set; } = string.Empty;
        public int     NombreAffectations { get; set; }
        public int     ActifsActifs       { get; set; }
    }

    public class EmployeDetailDto
    {
        public int     IdEmploye          { get; set; }
        public string  Nom                { get; set; } = string.Empty;
        public string  Prenom             { get; set; } = string.Empty;
        public string? Email              { get; set; }
        public string? Telephone          { get; set; }
        public int?    IdService          { get; set; }
        public string  NomService         { get; set; } = string.Empty;
        public int     NombreAffectations { get; set; }
        public int     ActifsActifs       { get; set; }
    }

    public class EmployeCreateDto
    {
        public string  Nom       { get; set; } = string.Empty;
        public string  Prenom    { get; set; } = string.Empty;
        public string? Email     { get; set; }
        public string? Telephone { get; set; }
        public int?    IdService { get; set; }
    }

    public class EmployeEditDto
    {
        public int     IdEmploye { get; set; }
        public string  Nom       { get; set; } = string.Empty;
        public string  Prenom    { get; set; } = string.Empty;
        public string? Email     { get; set; }
        public string? Telephone { get; set; }
        public int?    IdService { get; set; }
    }
}
