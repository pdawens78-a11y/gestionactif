using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IRapportService
    {
        Task<RapportDto> GetRapportAsync();
    }
}
