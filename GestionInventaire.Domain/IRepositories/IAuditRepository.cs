using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.IRepositories
{
    public interface IAuditRepository
    {
        void Log(string action, string entite, int entiteId);
        List<AuditLog> GetLogs();
        List<AuditLog> Rechercher(string? query, string? action, DateTime? dateDebut, DateTime? dateFin);
    }
}