using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class ActifService : IActifService
    {
        private readonly IActifRepository          _actifRepository;
        private readonly IProduitRepository        _produitRepository;
        private readonly ICategorieRepository      _categorieRepository;
        private readonly IStockRepository          _stockRepository;
        private readonly IMouvementStockRepository _mouvementRepository;
        private readonly IAuditRepository          _auditRepository;

        public ActifService(
            IActifRepository          actifRepository,
            IProduitRepository        produitRepository,
            ICategorieRepository      categorieRepository,
            IStockRepository          stockRepository,
            IMouvementStockRepository mouvementRepository,
            IAuditRepository          auditRepository)
        {
            _actifRepository     = actifRepository;
            _produitRepository   = produitRepository;
            _categorieRepository = categorieRepository;
            _stockRepository     = stockRepository;
            _mouvementRepository = mouvementRepository;
            _auditRepository     = auditRepository;
        }

        // ════════════════════════════════════════════
        // LECTURE
        // ════════════════════════════════════════════

        public async Task<IEnumerable<Actif>> GetAllActifsAsync()
            => await _actifRepository.GetAllAsync();

        public async Task<Actif> GetActifByIdAsync(int id)
        {
            return await _actifRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"L'actif #{id} n'existe pas.");
        }

        public async Task<IEnumerable<Actif>> GetActifsByProductAsync(int productId)
        {
            var all = await _actifRepository.GetAllAsync();
            return all.Where(a => a.IdProduit == productId).ToList();
        }

        public async Task<IEnumerable<Actif>> GetActifsByLocalisationAsync(string localisation)
        {
            var all = await _actifRepository.GetAllAsync();
            return all.Where(a => a.Localisation.Contains(localisation)).ToList();
        }

        public async Task<IEnumerable<Actif>> GetActifsByStatusAsync(StatutActif status)
        {
            var all = await _actifRepository.GetAllAsync();
            return all.Where(a => a.Statut == status).ToList();
        }

        public async Task<ActifListDto> GetAllActifsDtoAsync()
        {
            var actifs     = await _actifRepository.GetAllAsync();
            var produits   = await _produitRepository.GetAllAsync();
            var categories = await _categorieRepository.GetAllAsync();

            var produitsParId   = produits.ToDictionary(p => p.IdProduit);
            var categoriesParId = categories.ToDictionary(c => c.IdCategorie, c => c.NomCategorie);

            var items = actifs
                .OrderBy(a => a.CodeInventaire)
                .Select(a =>
                {
                    produitsParId.TryGetValue(a.IdProduit, out var produit);
                    var nomCategorie = produit != null
                        && categoriesParId.TryGetValue(produit.IdCategorie, out var nc)
                        ? nc : "—";

                    return new ActifItemDto
                    {
                        IdActif         = a.IdActif,
                        CodeInventaire  = a.CodeInventaire,
                        NomProduit      = produit?.NomProduit ?? $"Produit #{a.IdProduit}",
                        NomCategorie    = nomCategorie,
                        Localisation    = a.Localisation,
                        Statut          = a.Statut.ToString(),
                        DateAcquisition = a.DateAcquisition
                    };
                })
                .ToList();

            return new ActifListDto
            {
                Actifs             = items,
                TotalCount         = items.Count,
                TotalDisponibles   = items.Count(i => i.Statut == "Disponible"),
                TotalAffectes      = items.Count(i => i.Statut == "Affecte"),
                TotalEnMaintenance = items.Count(i => i.Statut == "EnMaintenance"),
                TotalHorsService   = items.Count(i => i.Statut == "HorsService")
            };
        }

        public async Task<ActifEditDto> GetActifEditDtoAsync(int id)
        {
            var actif   = await GetActifByIdAsync(id);
            var produit = await _produitRepository.GetByIdAsync(actif.IdProduit);

            return new ActifEditDto
            {
                IdActif        = actif.IdActif,
                CodeInventaire = actif.CodeInventaire,
                NomProduit     = produit?.NomProduit ?? $"Produit #{actif.IdProduit}",
                Localisation   = actif.Localisation,
                Statut         = actif.Statut.ToString()
            };
        }

        // ════════════════════════════════════════════
        // MODIFICATION
        // ════════════════════════════════════════════

        public async Task UpdateActifDtoAsync(ActifUpdateDto dto)
        {
            var actif = await GetActifByIdAsync(dto.IdActif);

            if (string.IsNullOrWhiteSpace(dto.Localisation) || dto.Localisation.Trim().Length < 3)
                throw new ArgumentException("La localisation doit contenir au moins 3 caractères.");

            if (!Enum.TryParse<StatutActif>(dto.Statut, out var statut))
                throw new ArgumentException($"Statut invalide : {dto.Statut}.");

            if (statut == StatutActif.Affecte || statut == StatutActif.EnMaintenance)
                throw new InvalidOperationException(
                    "Les statuts 'Affecté' et 'En maintenance' sont gérés automatiquement.");

            actif.Localisation = dto.Localisation.Trim();
            actif.Statut       = statut;

            _actifRepository.Update(actif);
            await _actifRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Modification actif : {actif.CodeInventaire} — {dto.Localisation} / {dto.Statut}",
                "Actif", actif.IdActif);
        }

        // ════════════════════════════════════════════
        // APPROVISIONNEMENT EN MASSE
        // ════════════════════════════════════════════

        public async Task<ApprovisionnerResultDto> ApprovisionnerAsync(ApprovisionnerDto dto)
        {
            if (dto.Quantite <= 0 || dto.Quantite > 10000)
                throw new ArgumentException("La quantité doit être entre 1 et 10 000.");

            if (string.IsNullOrWhiteSpace(dto.Localisation) || dto.Localisation.Trim().Length < 3)
                throw new ArgumentException("La localisation doit contenir au moins 3 caractères.");

            var produit = await _produitRepository.GetByIdAsync(dto.IdProduit)
                ?? throw new InvalidOperationException($"Le produit #{dto.IdProduit} n'existe pas.");

            var allActifs    = await _actifRepository.GetAllAsync();
            var codesGeneres = new List<string>();

            char firstLetter = char.ToUpper(produit.NomProduit[0]);

            // Substring(1) — supporte D6 et au-delà
            var existingNumbers = allActifs
                .Where(a => a.IdProduit == dto.IdProduit
                         && !string.IsNullOrWhiteSpace(a.CodeInventaire)
                         && a.CodeInventaire.Length >= 2
                         && char.ToUpper(a.CodeInventaire[0]) == firstLetter)
                .Select(a =>
                {
                    if (int.TryParse(a.CodeInventaire.Substring(1), out int n))
                        return n;
                    return 0;
                })
                .Where(n => n > 0)
                .ToList();

            int nextNumber = existingNumbers.Count > 0
                ? existingNumbers.Max() + 1
                : 1;

            // Générer N actifs — format D6
            for (int i = 0; i < dto.Quantite; i++)
            {
                var code = $"{firstLetter}{nextNumber:D6}";
                nextNumber++;

                await _actifRepository.CreateAsync(new Actif
                {
                    CodeInventaire  = code,
                    Statut          = StatutActif.Disponible,
                    Localisation    = dto.Localisation.Trim(),
                    DateAcquisition = DateTime.Today,
                    IdProduit       = dto.IdProduit
                });

                codesGeneres.Add(code);
            }

            // 1 seul SaveAsync — transaction atomique
            await _actifRepository.SaveAsync();

            // Mettre à jour le stock
            var stocks = await _stockRepository.GetAllAsync();
            var stock  = stocks.FirstOrDefault(s => s.IdProduit == dto.IdProduit);

            if (stock != null)
            {
                stock.Quantite += dto.Quantite;
                _stockRepository.Update(stock);

                await _mouvementRepository.CreateAsync(new MouvementStock
                {
                    IdStock       = stock.IdStock,
                    Type          = TypeMouvement.Entree,
                    Quantite      = dto.Quantite,
                    DateMouvement = DateTime.Now
                });

                await _stockRepository.SaveAsync();
            }

            await _auditRepository.LogAsync(
                $"Approvisionnement : {dto.Quantite} actif(s) générés pour « {produit.NomProduit} »",
                "Actif", dto.IdProduit);

            return new ApprovisionnerResultDto
            {
                NombreGenere = dto.Quantite,
                NomProduit   = produit.NomProduit,
                CodesGeneres = codesGeneres
            };
        }

        // ════════════════════════════════════════════
        // CRUD existants
        // ════════════════════════════════════════════

        public async Task<Actif> CreateActifAsync(Actif actif)
        {
            var produit = await _produitRepository.GetByIdAsync(actif.IdProduit)
                ?? throw new InvalidOperationException($"Le produit #{actif.IdProduit} n'existe pas.");

            actif.CodeInventaire = await GenerateCodeInventaireAsync(actif.IdProduit, produit.NomProduit);
            actif.Statut         = StatutActif.Disponible;

            await _actifRepository.CreateAsync(actif);
            await _actifRepository.SaveAsync();

            await _auditRepository.LogAsync("Création Actif", "Actif", actif.IdActif);
            return actif;
        }

        public async Task<Actif> UpdateActifAsync(Actif actif)
        {
            var existing = await GetActifByIdAsync(actif.IdActif);

            actif.CodeInventaire = existing.CodeInventaire;
            actif.IdProduit      = existing.IdProduit;

            _actifRepository.Update(actif);
            await _actifRepository.SaveAsync();

            await _auditRepository.LogAsync("Modification Actif", "Actif", actif.IdActif);
            return actif;
        }

        public async Task DeleteActifAsync(int id)
        {
            var actif = await GetActifByIdAsync(id);

            if (actif.Statut == StatutActif.Affecte)
                throw new InvalidOperationException("Impossible de supprimer un actif affecté.");

            if (actif.Statut == StatutActif.EnMaintenance)
                throw new InvalidOperationException("Impossible de supprimer un actif en maintenance.");

            _actifRepository.Delete(actif);
            await _actifRepository.SaveAsync();

            await _auditRepository.LogAsync("Suppression Actif", "Actif", id);
        }

        // ── Helper ──
        private async Task<string> GenerateCodeInventaireAsync(int productId, string productName)
        {
            char firstLetter    = char.ToUpper(productName[0]);
            var  allActifs      = await _actifRepository.GetAllAsync();

            var existingNumbers = allActifs
                .Where(a => a.IdProduit == productId
                         && !string.IsNullOrWhiteSpace(a.CodeInventaire)
                         && a.CodeInventaire.Length >= 2
                         && char.ToUpper(a.CodeInventaire[0]) == firstLetter)
                .Select(a =>
                {
                    if (int.TryParse(a.CodeInventaire.Substring(1), out int n)) return n;
                    return 0;
                })
                .Where(n => n > 0)
                .ToList();

            int nextNumber = existingNumbers.Count > 0
                ? existingNumbers.Max() + 1
                : 1;

            return $"{firstLetter}{nextNumber:D6}";
        }
    }
}
