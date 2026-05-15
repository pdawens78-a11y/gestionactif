using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.IRepositories
{
    public interface IMouvementStockRepository: IGenericRepository<MouvementStock>
    {
        //Task<List<MouvementStock>> GetHistoriqueParActifAsync(int actifId);
    }
}