using GestionInventaire.DAL.Data;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionInventaire.DAL.Repositories
{
    public class AuditRepository : GenericRepository<AuditLog>, IAuditRepository    
    {
        public AuditRepository(AppDbContext context) : base(context)
        { 

        }

        public List<AuditLog> GetLogs()
        {
            throw new NotImplementedException();
        }

        public void Log(string action, string entite, int entiteId)
        {
            throw new NotImplementedException();
        }

        public List<AuditLog> Rechercher(string? query, string? action, DateTime? dateDebut, DateTime? dateFin)
        {
            throw new NotImplementedException();
        }
    }
}
