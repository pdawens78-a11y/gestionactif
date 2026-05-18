using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IUtilisateurService
    {
        Task<UtilisateurListDto> GetAllUtilisateursDtoAsync();
        Task<UtilisateurEditDto> GetUtilisateurByIdAsync(string id);
        Task<List<string>>       GetRolesDisponiblesAsync();
        Task                     UpdateUtilisateurAsync(UtilisateurUpdateDto dto);
        Task                     VerrouillerAsync(string id);
        Task                     DeverrouillerAsync(string id);
        Task                     SupprimerAsync(string id);
    }
}
