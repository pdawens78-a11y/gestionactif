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
    public class AffectationRepository : GenericRepository<Affectation>, IAffectationRepository
    {
        public AffectationRepository(AppDbContext context) : base(context)
        {

        }
    }
}
