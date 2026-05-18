using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IStockService
    {
        Task<StockListDto>       GetAllStocksDtoAsync();
        Task<StockEditDto>       GetStockByIdAsync(int id);
        Task                     UpdateStockAsync(StockUpdateDto dto);
        Task                     AjouterMouvementAsync(StockMouvementDto dto);
        Task<StockHistoriqueDto> GetHistoriqueAsync(int idStock);
    }
}
