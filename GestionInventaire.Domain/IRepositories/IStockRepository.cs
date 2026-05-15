using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.IRepositories
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        //Task AjouterStockAsync(int actifId, int quantite);
        //Task RetirerStockAsync(int actifId, int quantite);
        //Task<List<Stock>> GetStocksCritiquesAsync();
    }
}