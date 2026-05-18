using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IAffectationService
    {
        Task<AffectationListDto>    GetAllAffectationsDtoAsync();
        Task<AffectationFormDataDto> GetFormDataAsync();
        Task                        AffecterAsync(AffectationCreateDto dto, string currentUserId);
        Task                        RetournerAsync(int idAffectation, string currentUserId);
    }
}
