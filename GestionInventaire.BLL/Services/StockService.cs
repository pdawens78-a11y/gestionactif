using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMouvementStockRepository _mouvementRepository;
        private readonly IProduitRepository _produitRepository;
        private readonly ICategorieRepository _categorieRepository;
        private readonly IAuditRepository _auditRepository;

        public StockService(
            IStockRepository stockRepository,
            IMouvementStockRepository mouvementRepository,
            IProduitRepository produitRepository,
            ICategorieRepository categorieRepository,
            IAuditRepository auditRepository)
        {
            _stockRepository = stockRepository;
            _mouvementRepository = mouvementRepository;
            _produitRepository = produitRepository;
            _categorieRepository = categorieRepository;
            _auditRepository = auditRepository;
        }

        public async Task<StockListDto> GetAllStocksDtoAsync()
        {
            var stocks = await _stockRepository.GetAllAsync();
            var produits = await _produitRepository.GetAllAsync();
            var categories = await _categorieRepository.GetAllAsync();

            var produitsParId = produits.ToDictionary(p => p.IdProduit);
            var categoriesParId = categories.ToDictionary(c => c.IdCategorie, c => c.NomCategorie);

            var items = stocks
                .OrderBy(s => s.Quantite)
                .Select(s =>
                {
                    produitsParId.TryGetValue(s.IdProduit, out var produit);
                    var nomCategorie = produit != null
                        && categoriesParId.TryGetValue(produit.IdCategorie, out var nc)
                        ? nc : "—";

                    return new StockItemDto
                    {
                        IdStock = s.IdStock,
                        NomProduit = produit?.NomProduit ?? $"Produit #{s.IdProduit}",
                        NomCategorie = nomCategorie,
                        Quantite = s.Quantite,
                        SeuilAlerte = s.SeuilAlerte,
                        EstCritique = s.Quantite <= s.SeuilAlerte
                    };
                })
                .ToList();

            return new StockListDto
            {
                Stocks = items,
                TotalCount = items.Count,
                StocksCritiques = items.Count(i => i.EstCritique)
            };
        }

        public async Task<StockEditDto> GetStockByIdAsync(int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Le stock #{id} n'existe pas.");

            var produit = await _produitRepository.GetByIdAsync(stock.IdProduit);

            return new StockEditDto
            {
                IdStock = stock.IdStock,
                NomProduit = produit?.NomProduit ?? $"Produit #{stock.IdProduit}",
                Quantite = stock.Quantite,
                SeuilAlerte = stock.SeuilAlerte
            };
        }

        public async Task AjouterMouvementAsync(StockMouvementDto dto)
        {
            var stock = await _stockRepository.GetByIdAsync(dto.IdStock)
                ?? throw new InvalidOperationException($"Le stock #{dto.IdStock} n'existe pas.");

            if (!Enum.TryParse<TypeMouvement>(dto.Type, out var type))
                throw new ArgumentException($"Type de mouvement invalide : {dto.Type}.");

            if (dto.Quantite <= 0)
                throw new ArgumentException("La quantité doit être supérieure à 0.");

            if (type == TypeMouvement.Sortie && stock.Quantite < dto.Quantite)
                throw new InvalidOperationException(
                    $"Stock insuffisant : {stock.Quantite} disponible(s), {dto.Quantite} demandé(s).");

            stock.Quantite = type == TypeMouvement.Entree
                ? stock.Quantite + dto.Quantite
                : stock.Quantite - dto.Quantite;

            _stockRepository.Update(stock);

            await _mouvementRepository.CreateAsync(new MouvementStock
            {
                IdStock = dto.IdStock,
                Type = type,
                Quantite = dto.Quantite,
                DateMouvement = DateTime.Now
            });

            await _stockRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Mouvement {dto.Type} stock #{dto.IdStock} : {dto.Quantite} unité(s)",
                "Stock", dto.IdStock);
        }

        public async Task<StockHistoriqueDto> GetHistoriqueAsync(int idStock)
        {
            var stock = await _stockRepository.GetByIdAsync(idStock)
                ?? throw new InvalidOperationException($"Le stock #{idStock} n'existe pas.");

            var mouvements = await _mouvementRepository.GetAllAsync();
            var produit = await _produitRepository.GetByIdAsync(stock.IdProduit);
            var categories = await _categorieRepository.GetAllAsync();

            var nomCategorie = "—";
            if (produit != null)
            {
                var cat = categories.FirstOrDefault(c => c.IdCategorie == produit.IdCategorie);
                nomCategorie = cat?.NomCategorie ?? "—";
            }

            var items = mouvements
                .Where(m => m.IdStock == idStock)
                .OrderByDescending(m => m.DateMouvement)
                .Select(m => new MouvementItemDto
                {
                    IdMouvement = m.IdMouvement,
                    DateMouvement = m.DateMouvement,
                    Type = m.Type.ToString(),
                    Quantite = m.Quantite
                })
                .ToList();

            return new StockHistoriqueDto
            {
                IdStock = idStock,
                NomProduit = produit?.NomProduit ?? $"Produit #{stock.IdProduit}",
                NomCategorie = nomCategorie,
                QuantiteActuelle = stock.Quantite,
                SeuilAlerte = stock.SeuilAlerte,
                Mouvements = items
            };
        }
    }
}