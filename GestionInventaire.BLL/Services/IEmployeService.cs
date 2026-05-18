using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IEmployeService
    {
        Task<EmployeListDto>   GetAllEmployesDtoAsync();
        Task<EmployeDetailDto> GetEmployeByIdAsync(int id);
        Task                   CreateEmployeAsync(EmployeCreateDto dto);
        Task                   UpdateEmployeAsync(EmployeEditDto dto);
        Task                   DeleteEmployeAsync(int id);
    }
}
