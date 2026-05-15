using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.IRepositories
{
    public interface IMaintenanceRepository : IGenericRepository<Maintenance>
    {
        //Task<List<Maintenance>> GetMaintenancesEnCoursAsync();
        //Task<List<Maintenance>> GetMaintenancesParActifAsync(int actifId);


        //Task PlanifierMaintenanceAsync(int actifId, DateTime date, decimal cout);
        //Task DemarrerMaintenanceAsync(int maintenanceId);
        //Task TerminerMaintenanceAsync(int maintenanceId);

        //Task<List<Maintenance>> RechercherAsync(string? query, string? statut);
    }
}