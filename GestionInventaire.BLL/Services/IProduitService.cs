using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IProduitService
    {
        Task<ProduitListDto>         GetAllProduitsDtoAsync();
        Task<ProduitDetailDto>       GetProduitByIdAsync(int id);
        Task<ProduitFormDataDto>     GetFormDataAsync();
        Task<ProduitCreateResultDto> CreateProduitAsync(ProduitCreateDto dto);
        Task                         UpdateProduitAsync(ProduitEditDto dto);
        Task                         DeleteProduitAsync(int id);
    }
}
