using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionInventaire.BLL.Services
{
    public interface IActifService
    {
        Task<IEnumerable<Actif>> GetAllActifsAsync();
        Task<Actif> GetActifByIdAsync(int id);
        Task<Actif> CreateActifAsync(Actif actif);
        Task<Actif> UpdateActifAsync(Actif actif);
        Task DeleteActifAsync(int id);
        Task<IEnumerable<Actif>> GetActifsByProductAsync(int productId);
        Task<IEnumerable<Actif>> GetActifsByLocalisationAsync(string localisation);
        Task<IEnumerable<Actif>> GetActifsByStatusAsync(StatutActif status);
    }
}
