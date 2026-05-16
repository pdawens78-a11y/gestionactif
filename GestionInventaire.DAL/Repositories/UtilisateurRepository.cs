using GestionInventaire.DAL.Data;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;


namespace GestionInventaire.DAL.Repositories
{
    public class UtilisateurRepository : GenericRepository<Utilisateur>, IUtilisateurRepository
    {
        public UtilisateurRepository(AppDbContext context) : base(context)
        {
        }
    }
}
