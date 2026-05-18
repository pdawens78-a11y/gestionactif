namespace GestionInventaire.BLL.Dtos
{
    public class UtilisateurListDto
    {
        public List<UtilisateurItemDto> Utilisateurs { get; set; } = new();
        public int                      TotalCount   { get; set; }
    }

    public class UtilisateurItemDto
    {
        public string  Id             { get; set; } = string.Empty;
        public string  Nom            { get; set; } = string.Empty;
        public string  Prenom         { get; set; } = string.Empty;
        public string  NomComplet     { get; set; } = string.Empty;
        public string  Email          { get; set; } = string.Empty;
        public string? Telephone      { get; set; }
        public string  Role           { get; set; } = string.Empty;
        public bool    EstVerrouille  { get; set; }
        public bool    EmailConfirme  { get; set; }
    }

    public class UtilisateurEditDto
    {
        public string  Id        { get; set; } = string.Empty;
        public string  Nom       { get; set; } = string.Empty;
        public string  Prenom    { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string  Role      { get; set; } = string.Empty;
        public string  Email     { get; set; } = string.Empty;
    }

    public class UtilisateurUpdateDto
    {
        public string  Id        { get; set; } = string.Empty;
        public string  Nom       { get; set; } = string.Empty;
        public string  Prenom    { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string  Role      { get; set; } = string.Empty;
    }
}
