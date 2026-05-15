using GestionInventaire.DAL.Data;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.DAL.Repositories
{
    public class ProduitRepository : GenericRepository<Produit>, IProduitRepository
    {
        public ProduitRepository(AppDbContext context) : base(context)
        {
        }
    }
}