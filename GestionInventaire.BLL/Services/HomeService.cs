using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class HomeService : IHomeService
    {
        private readonly IActifService      _actifService;
        private readonly IProduitRepository _produitRepository;
        private readonly IEmployeRepository _employeRepository;
        private readonly IStockRepository   _stockRepository;

        public HomeService(
            IActifService      actifService,
            IProduitRepository produitRepository,
            IEmployeRepository employeRepository,
            IStockRepository   stockRepository)
        {
            _actifService      = actifService;
            _produitRepository = produitRepository;
            _employeRepository = employeRepository;
            _stockRepository   = stockRepository;
        }

        public async Task<HomeDto> GetHomeDtoAsync()
        {
            var actifs    = (await _actifService.GetAllActifsAsync()).ToList();
            var produits  = await _produitRepository.GetAllAsync();
            var employes  = await _employeRepository.GetAllAsync();
            var stocks    = await _stockRepository.GetAllAsync();

            return new HomeDto
            {
                TotalActifs     = actifs.Count,
                TotalProduits   = produits.Count(),
                TotalEmployes   = employes.Count(),
                StocksCritiques = stocks.Count(s => s.Quantite <= s.SeuilAlerte)
            };
        }
    }
}
