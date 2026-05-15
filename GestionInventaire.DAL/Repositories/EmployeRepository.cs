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
    public class EmployeRepository : GenericRepository<Employe>, IEmployeRepository
    {
        public EmployeRepository(AppDbContext context) : base(context)
        {
        }
        //public Task<List<Employe>> RechercherAsync(string? query, string? service)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
