using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IMaintenanceService
    {
        Task<MaintenanceListDto>   GetAllMaintenancesDtoAsync();
        Task<MaintenanceDetailDto> GetMaintenanceByIdAsync(int id);
        Task<List<ActifDisponibleMaintenanceDto>> GetActifsForMaintenanceAsync();
        Task                       CreateMaintenanceAsync(MaintenanceCreateDto dto);
        Task                       UpdateMaintenanceAsync(MaintenanceEditDto dto);
        Task                       DeleteMaintenanceAsync(int id);
        Task                       ChangerStatutAsync(int id, string nouveauStatut);
    }
}
