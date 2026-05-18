using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IAuditService
    {
        Task<AuditListDto> GetLogsAsync();
        Task<AuditListDto> RechercherAsync(AuditFiltreDto filtre);
    }
}
