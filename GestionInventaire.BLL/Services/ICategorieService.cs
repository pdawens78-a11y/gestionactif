using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface ICategorieService
    {
        Task<CategorieListDto>   GetAllCategoriesDtoAsync();
        Task<CategorieDetailDto> GetCategorieByIdAsync(int id);
        Task                     CreateCategorieAsync(CategorieCreateDto dto);
        Task                     UpdateCategorieAsync(CategorieEditDto dto);
        Task                     DeleteCategorieAsync(int id);
    }
}
