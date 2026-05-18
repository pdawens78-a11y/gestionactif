using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.Enums;

namespace GestionInventaire.BLL.Services
{
    public interface IActifService
    {
        // ── Existants ──
        Task<IEnumerable<Actif>> GetAllActifsAsync();
        Task<Actif>              GetActifByIdAsync(int id);
        Task<Actif>              CreateActifAsync(Actif actif);
        Task<Actif>              UpdateActifAsync(Actif actif);
        Task                     DeleteActifAsync(int id);
        Task<IEnumerable<Actif>> GetActifsByProductAsync(int productId);
        Task<IEnumerable<Actif>> GetActifsByLocalisationAsync(string localisation);
        Task<IEnumerable<Actif>> GetActifsByStatusAsync(StatutActif status);

        // ── DTOs ──
        Task<ActifListDto>            GetAllActifsDtoAsync();
        Task<ActifEditDto>            GetActifEditDtoAsync(int id);
        Task                          UpdateActifDtoAsync(ActifUpdateDto dto);
        Task<ApprovisionnerResultDto> ApprovisionnerAsync(ApprovisionnerDto dto);
    }
}
