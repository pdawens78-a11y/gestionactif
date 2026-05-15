using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.IRepositories
{   
    public interface IAffectationRepository : IGenericRepository<Affectation>
    {
        //Task AffecterAsync(int actifId, int employeId);
        //Task<Affectation?> RetournerAsync(int affectationId);
        //Task<Affectation?> RetournerParActifAsync(int actifId);

        //Task<List<Affectation>> GetByEmployeAsync(int employeId);
        //Task<List<Affectation>> GetByActifAsync(int actifId);

        //Task<List<Affectation>> RechercherAsync(string? query, string? statut);
    }
}