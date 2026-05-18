using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.IRepositories
{
    public interface IAuditRepository
    {
        void Log(string action, string entite, int entiteId);
        Task LogAsync(string action, string entite, int entiteId);
        List<AuditLog> GetLogs();
        Task<List<AuditLog>> GetLogsAsync();
        List<AuditLog> Rechercher(string? query, string? action, DateTime? dateDebut, DateTime? dateFin);
        Task<List<AuditLog>> RechercherAsync(string? query, string? action, DateTime? dateDebut, DateTime? dateFin);
    }
}