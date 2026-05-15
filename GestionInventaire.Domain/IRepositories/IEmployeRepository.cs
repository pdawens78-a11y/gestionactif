using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.IRepositories
{
    public interface IEmployeRepository : IGenericRepository<Employe>
    {

        //Task<List<Employe>> RechercherAsync(string? query, string? service);
    }
}