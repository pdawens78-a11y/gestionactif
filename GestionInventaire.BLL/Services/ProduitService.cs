using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class ProduitService : IProduitService
    {
        private readonly IProduitRepository _produitRepository;
        private readonly ICategorieRepository _categorieRepository;
        private readonly IActifRepository _actifRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMouvementStockRepository _mouvementRepository;
        private readonly IAuditRepository _auditRepository;

        public ProduitService(
            IProduitRepository produitRepository,
            ICategorieRepository categorieRepository,
            IActifRepository actifRepository,
            IStockRepository stockRepository,
            IMouvementStockRepository mouvementRepository,
            IAuditRepository auditRepository)
        {
            _produitRepository = produitRepository;
            _categorieRepository = categorieRepository;
            _actifRepository = actifRepository;
            _stockRepository = stockRepository;
            _mouvementRepository = mouvementRepository;
            _auditRepository = auditRepository;
        }

        // ════════════════════════════════════════════
        // LISTE
        // ════════════════════════════════════════════

        public async Task<ProduitListDto> GetAllProduitsDtoAsync()
        {
            var produits = await _produitRepository.GetAllAsync();
            var categories = await _categorieRepository.GetAllAsync();
            var actifs = await _actifRepository.GetAllAsync();
            var stocks = await _stockRepository.GetAllAsync();

            var categoriesParId = categories.ToDictionary(c => c.IdCategorie, c => c.NomCategorie);
            var actifsParProduit = actifs
                .GroupBy(a => a.IdProduit)
                .ToDictionary(g => g.Key, g => g.Count());
            var stocksParProduit = stocks.ToDictionary(s => s.IdProduit);

            var items = produits
                .OrderBy(p => p.NomProduit)
                .Select(p =>
                {
                    stocksParProduit.TryGetValue(p.IdProduit, out var stock);
                    return new ProduitItemDto
                    {
                        IdProduit = p.IdProduit,
                        NomProduit = p.NomProduit,
                        Description = p.Description,
                        IdCategorie = p.IdCategorie,
                        NomCategorie = categoriesParId.TryGetValue(p.IdCategorie, out var nc)
                            ? nc : $"Catégorie #{p.IdCategorie}",
                        NombreActifs = actifsParProduit.TryGetValue(p.IdProduit, out var cnt) ? cnt : 0,
                        StockQuantite = stock?.Quantite,
                        StockSeuil = stock?.SeuilAlerte,
                        StockCritique = stock != null && stock.Quantite <= stock.SeuilAlerte
                    };
                })
                .ToList();

            return new ProduitListDto { Produits = items, TotalCount = items.Count };
        }

        // ════════════════════════════════════════════
        // DÉTAIL
        // ════════════════════════════════════════════

        public async Task<ProduitDetailDto> GetProduitByIdAsync(int id)
        {
            var produit = await _produitRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Le produit #{id} n'existe pas.");

            var categorie = await _categorieRepository.GetByIdAsync(produit.IdCategorie);
            var actifs = await _actifRepository.GetAllAsync();
            var stocks = await _stockRepository.GetAllAsync();
            var stock = stocks.FirstOrDefault(s => s.IdProduit == id);

            return new ProduitDetailDto
            {
                IdProduit = produit.IdProduit,
                NomProduit = produit.NomProduit,
                Description = produit.Description,
                IdCategorie = produit.IdCategorie,
                NomCategorie = categorie?.NomCategorie ?? $"Catégorie #{produit.IdCategorie}",
                NombreActifs = actifs.Count(a => a.IdProduit == id),
                IdStock = stock?.IdStock,
                StockQuantite = stock?.Quantite,
                StockSeuilAlerte = stock?.SeuilAlerte
            };
        }

        // ════════════════════════════════════════════
        // FORM DATA
        // ════════════════════════════════════════════

        public async Task<ProduitFormDataDto> GetFormDataAsync()
        {
            var categories = await _categorieRepository.GetAllAsync();
            return new ProduitFormDataDto
            {
                Categories = categories
                    .OrderBy(c => c.NomCategorie)
                    .Select(c => new CategorieSelectDto
                    {
                        IdCategorie = c.IdCategorie,
                        NomCategorie = c.NomCategorie
                    })
                    .ToList()
            };
        }

        // ════════════════════════════════════════════
        // CRÉATION
        // ════════════════════════════════════════════

        public async Task<ProduitCreateResultDto> CreateProduitAsync(ProduitCreateDto dto)
        {
            await ValidateCreateAsync(dto);

            // ── SaveAsync #1 — obtenir IdProduit ──
            var produit = new Produit
            {
                NomProduit = dto.NomProduit.Trim(),
                Description = dto.Description?.Trim(),
                IdCategorie = dto.IdCategorie
            };

            await _produitRepository.CreateAsync(produit);
            await _produitRepository.SaveAsync();

            // ── Créer le Stock ──
            var stock = new Stock
            {
                IdProduit = produit.IdProduit,
                Quantite = dto.QuantiteActifs,
                SeuilAlerte = dto.SeuilAlerte
            };

            await _stockRepository.CreateAsync(stock);

            // ── Générer les actifs ──
            var codesGeneres = new List<string>();
            char firstLetter = char.ToUpper(produit.NomProduit[0]);
            int nextNumber = 1;

            for (int i = 0; i < dto.QuantiteActifs; i++)
            {
                var code = $"{firstLetter}{nextNumber:D6}";
                nextNumber++;

                await _actifRepository.CreateAsync(new Actif
                {
                    CodeInventaire = code,
                    Statut = StatutActif.Disponible,
                    Localisation = dto.Localisation.Trim(),
                    DateAcquisition = DateTime.Today,
                    IdProduit = produit.IdProduit
                });

                codesGeneres.Add(code);
            }

            // ── SaveAsync #2 — Stock + Actifs ──
            await _stockRepository.SaveAsync();

            // ── SaveAsync #3 — Mouvement d'entrée ──
            if (dto.QuantiteActifs > 0)
            {
                await _mouvementRepository.CreateAsync(new MouvementStock
                {
                    IdStock = stock.IdStock,
                    Type = TypeMouvement.Entree,
                    Quantite = dto.QuantiteActifs,
                    DateMouvement = DateTime.Now
                });

                await _mouvementRepository.SaveAsync();
            }

            await _auditRepository.LogAsync(
                $"Création produit « {produit.NomProduit} » + {dto.QuantiteActifs} actif(s)",
                "Produit", produit.IdProduit);

            return new ProduitCreateResultDto
            {
                IdProduit = produit.IdProduit,
                NomProduit = produit.NomProduit,
                NombreActifs = dto.QuantiteActifs,
                StockQuantite = dto.QuantiteActifs,
                CodesGeneres = codesGeneres
            };
        }

        // ════════════════════════════════════════════
        // MODIFICATION
        // ════════════════════════════════════════════

        public async Task UpdateProduitAsync(ProduitEditDto dto)
        {
            var existing = await _produitRepository.GetByIdAsync(dto.IdProduit)
                ?? throw new InvalidOperationException($"Le produit #{dto.IdProduit} n'existe pas.");

            await ValidateNomAsync(dto.NomProduit, dto.IdCategorie, dto.IdProduit);

            // ── Mise à jour du produit uniquement ──
            existing.NomProduit = dto.NomProduit.Trim();
            existing.Description = dto.Description?.Trim();
            existing.IdCategorie = dto.IdCategorie;

            _produitRepository.Update(existing);
            await _produitRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Modification produit : {existing.NomProduit}",
                "Produit", existing.IdProduit);
        }

        // ════════════════════════════════════════════
        // SUPPRESSION
        // ════════════════════════════════════════════

        public async Task DeleteProduitAsync(int id)
        {
            var produit = await _produitRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Le produit #{id} n'existe pas.");

            var actifs = await _actifRepository.GetAllAsync();
            var actifsActifs = actifs
                .Where(a => a.IdProduit == id && a.Statut != StatutActif.HorsService)
                .ToList();

            if (actifsActifs.Any())
                throw new InvalidOperationException(
                    $"Impossible de supprimer « {produit.NomProduit} » : " +
                    $"{actifsActifs.Count} actif(s) encore en service.");

            var stocks = await _stockRepository.GetAllAsync();
            var stock = stocks.FirstOrDefault(s => s.IdProduit == id);

            if (stock != null)
            {
                var mouvements = await _mouvementRepository.GetAllAsync();
                foreach (var m in mouvements.Where(m => m.IdStock == stock.IdStock))
                    _mouvementRepository.Delete(m);

                _stockRepository.Delete(stock);
            }

            foreach (var a in actifs.Where(a => a.IdProduit == id))
                _actifRepository.Delete(a);

            _produitRepository.Delete(produit);
            await _produitRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Suppression produit : {produit.NomProduit}", "Produit", id);
        }

        // ════════════════════════════════════════════
        // VALIDATION
        // ════════════════════════════════════════════

        private async Task ValidateCreateAsync(ProduitCreateDto dto)
        {
            await ValidateNomAsync(dto.NomProduit, dto.IdCategorie);

            if (dto.QuantiteActifs < 0)
                throw new ArgumentException("La quantité d'actifs ne peut pas être négative.");

            if (dto.QuantiteActifs > 10000)
                throw new ArgumentException("La quantité ne peut pas dépasser 10 000.");

            if (dto.QuantiteActifs > 0
                && (string.IsNullOrWhiteSpace(dto.Localisation) || dto.Localisation.Trim().Length < 3))
                throw new ArgumentException("La localisation est obligatoire (min. 3 caractères).");

            if (dto.SeuilAlerte < 0)
                throw new ArgumentException("Le seuil d'alerte ne peut pas être négatif.");
        }

        private async Task ValidateNomAsync(string nom, int idCategorie, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(nom) || nom.Trim().Length < 2)
                throw new ArgumentException("Le nom du produit est obligatoire (min. 2 caractères).");

            var categorie = await _categorieRepository.GetByIdAsync(idCategorie);
            if (categorie == null)
                throw new InvalidOperationException("La catégorie sélectionnée n'existe pas.");

            var produits = await _produitRepository.GetAllAsync();
            var duplicate = produits.FirstOrDefault(p =>
                p.NomProduit.Trim().ToLower() == nom.Trim().ToLower()
                && p.IdCategorie == idCategorie
                && p.IdProduit != (excludeId ?? 0));

            if (duplicate != null)
                throw new InvalidOperationException(
                    $"Un produit nommé « {nom.Trim()} » existe déjà dans cette catégorie.");
        }
    }
}