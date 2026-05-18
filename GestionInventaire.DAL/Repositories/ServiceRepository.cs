using GestionInventaire.DAL.Data;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.DAL.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext context) : base(context)
        {
        }
    }
}
