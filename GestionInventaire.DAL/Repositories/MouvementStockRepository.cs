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
    public class MouvementStockRepository : GenericRepository<MouvementStock>, IMouvementStockRepository
    {
        public MouvementStockRepository(AppDbContext context) : base(context)
        {
        }
        //public async Task<List<MouvementStock>> GetHistoriqueParActifAsync(int actifId)
        //{
        //    return await _context.MouvementsStock
        //                         .Where(m => m.ActifId == actifId)
        //                         .OrderByDescending(m => m.DateMouvement)
        //                         .ToListAsync();
        //}
    }
}
