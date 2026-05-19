using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class HomeService : IHomeService
    {
        private readonly IActifRepository _actifRepository;
        private readonly IProduitRepository _produitRepository;
        private readonly IEmployeRepository _employeRepository;
        private readonly IStockRepository _stockRepository;

        public HomeService(
            IActifRepository actifRepository,
            IProduitRepository produitRepository,
            IEmployeRepository employeRepository,
            IStockRepository stockRepository)
        {
            _actifRepository = actifRepository;
            _produitRepository = produitRepository;
            _employeRepository = employeRepository;
            _stockRepository = stockRepository;
        }

        public async Task<HomeDto> GetHomeDtoAsync()
        {
            var actifs = (await _actifRepository.GetAllAsync()).ToList();
            var produits = await _produitRepository.GetAllAsync();
            var employes = await _employeRepository.GetAllAsync();
            var stocks = await _stockRepository.GetAllAsync();

            return new HomeDto
            {
                TotalActifs = actifs.Count,
                ActifsDisponibles = actifs.Count(a => a.Statut == StatutActif.Disponible),
                ActifsAffectes = actifs.Count(a => a.Statut == StatutActif.Affecte),
                ActifsEnMaintenance = actifs.Count(a => a.Statut == StatutActif.EnMaintenance),
                TotalProduits = produits.Count(),
                TotalEmployes = employes.Count(),
                StocksCritiques = stocks.Count(s => s.Quantite <= s.SeuilAlerte)
            };
        }
    }
}