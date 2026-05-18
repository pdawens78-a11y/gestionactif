using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDtoAsync();
    }
}
