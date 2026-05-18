namespace GestionInventaire.BLL.Dtos
{
    public class AuditListDto
    {
        public List<AuditLogItemDto> Logs       { get; set; } = new();
        public int                   TotalCount { get; set; }
    }

    public class AuditLogItemDto
    {
        public int      IdAuditLog     { get; set; }
        public string   Action         { get; set; } = string.Empty;
        public string   Entite         { get; set; } = string.Empty;
        public int      EntiteId       { get; set; }
        public DateTime DateAction     { get; set; }
        public string   IdUtilisateur  { get; set; } = string.Empty;
        public string   UtilisateurNom { get; set; } = string.Empty;
    }

    public class AuditFiltreDto
    {
        public string?   Query     { get; set; }
        public string?   Action    { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin   { get; set; }
    }
}
