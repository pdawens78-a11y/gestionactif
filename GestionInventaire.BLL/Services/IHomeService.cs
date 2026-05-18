using GestionInventaire.BLL.Dtos;

namespace GestionInventaire.BLL.Services
{
    public interface IHomeService
    {
        Task<HomeDto> GetHomeDtoAsync();
    }
}
