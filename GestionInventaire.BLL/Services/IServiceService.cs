using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IServiceService
    {
        Task<ServiceListDto>         GetAllServicesDtoAsync();
        Task<ServiceDetailDto>       GetServiceByIdAsync(int id);
        Task<List<ServiceSelectDto>> GetServicesSelectAsync();
        Task                         CreateServiceAsync(ServiceCreateDto dto);
        Task                         UpdateServiceAsync(ServiceEditDto dto);
        Task                         DeleteServiceAsync(int id);
    }
}
