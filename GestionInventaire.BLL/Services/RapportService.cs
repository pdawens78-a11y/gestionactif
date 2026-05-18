using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class RapportService : IRapportService
    {
        private readonly IActifService          _actifService;
        private readonly IProduitRepository     _produitRepository;
        private readonly ICategorieRepository   _categorieRepository;
        private readonly IStockRepository       _stockRepository;
        private readonly IAffectationRepository _affectationRepository;
        private readonly IEmployeRepository     _employeRepository;
        private readonly IServiceRepository     _serviceRepository;
        private readonly IMaintenanceRepository _maintenanceRepository;

        public RapportService(
            IActifService          actifService,
            IProduitRepository     produitRepository,
            ICategorieRepository   categorieRepository,
            IStockRepository       stockRepository,
            IAffectationRepository affectationRepository,
            IEmployeRepository     employeRepository,
            IServiceRepository     serviceRepository,
            IMaintenanceRepository maintenanceRepository)
        {
            _actifService          = actifService;
            _produitRepository     = produitRepository;
            _categorieRepository   = categorieRepository;
            _stockRepository       = stockRepository;
            _affectationRepository = affectationRepository;
            _employeRepository     = employeRepository;
            _serviceRepository     = serviceRepository;
            _maintenanceRepository = maintenanceRepository;
        }

        public async Task<RapportDto> GetRapportAsync()
        {
            // ── Séquentiel — EF Core interdit Task.WhenAll sur le même DbContext ──
            var actifs       = (await _actifService.GetAllActifsAsync()).ToList();
            var produits     = await _produitRepository.GetAllAsync();
            var categories   = await _categorieRepository.GetAllAsync();
            var stocks       = await _stockRepository.GetAllAsync();
            var affectations = await _affectationRepository.GetAllAsync();
            var employes     = await _employeRepository.GetAllAsync();
            var services     = await _serviceRepository.GetAllAsync();
            var maintenances = await _maintenanceRepository.GetAllAsync();

            var produitsParId   = produits.ToDictionary(p => p.IdProduit);
            var categoriesParId = categories.ToDictionary(c => c.IdCategorie, c => c.NomCategorie);
            var employesParId   = employes.ToDictionary(e => e.IdEmploye);
            var servicesParId   = services.ToDictionary(s => s.IdService, s => s.NomService);

            return new RapportDto
            {
                DateGeneration = DateTime.Now,
                Inventaire     = BuildInventaire(actifs, produitsParId, categoriesParId),
                Stock          = BuildStock(stocks, produitsParId, categoriesParId),
                Affectations   = BuildAffectations(affectations, actifs, produitsParId, employesParId, servicesParId),
                Maintenances   = BuildMaintenances(maintenances, actifs, produitsParId)
            };
        }

        private static RapportInventaireDto BuildInventaire(
            List<Domain.Entities.Actif>              actifs,
            Dictionary<int, Domain.Entities.Produit> produitsParId,
            Dictionary<int, string>                  categoriesParId)
        {
            var lignes = actifs
                .OrderBy(a => a.CodeInventaire)
                .Select(a =>
                {
                    produitsParId.TryGetValue(a.IdProduit, out var produit);
                    var nomCategorie = produit != null
                        && categoriesParId.TryGetValue(produit.IdCategorie, out var nc) ? nc : "—";

                    return new RapportActifLigneDto
                    {
                        CodeInventaire  = a.CodeInventaire,
                        NomProduit      = produit?.NomProduit ?? $"Produit #{a.IdProduit}",
                        NomCategorie    = nomCategorie,
                        Localisation    = a.Localisation,
                        Statut          = a.Statut.ToString(),
                        DateAcquisition = a.DateAcquisition
                    };
                })
                .ToList();

            return new RapportInventaireDto
            {
                TotalActifs        = actifs.Count,
                TotalDisponibles   = actifs.Count(a => a.Statut == StatutActif.Disponible),
                TotalAffectes      = actifs.Count(a => a.Statut == StatutActif.Affecte),
                TotalEnMaintenance = actifs.Count(a => a.Statut == StatutActif.EnMaintenance),
                TotalHorsService   = actifs.Count(a => a.Statut == StatutActif.HorsService),
                Actifs             = lignes
            };
        }

        private static RapportStockDto BuildStock(
            IEnumerable<Domain.Entities.Stock>       stocks,
            Dictionary<int, Domain.Entities.Produit> produitsParId,
            Dictionary<int, string>                  categoriesParId)
        {
            var lignes = stocks
                .OrderBy(s => s.Quantite)
                .Select(s =>
                {
                    produitsParId.TryGetValue(s.IdProduit, out var produit);
                    var nomCategorie = produit != null
                        && categoriesParId.TryGetValue(produit.IdCategorie, out var nc) ? nc : "—";

                    return new RapportStockLigneDto
                    {
                        NomProduit   = produit?.NomProduit ?? $"Produit #{s.IdProduit}",
                        NomCategorie = nomCategorie,
                        Quantite     = s.Quantite,
                        SeuilAlerte  = s.SeuilAlerte,
                        EstCritique  = s.Quantite <= s.SeuilAlerte && s.Quantite > 0,
                        EstEpuise    = s.Quantite == 0
                    };
                })
                .ToList();

            return new RapportStockDto
            {
                TotalProduits   = lignes.Count,
                StocksCritiques = lignes.Count(l => l.EstCritique),
                StocksEpuises   = lignes.Count(l => l.EstEpuise),
                Stocks          = lignes
            };
        }

        private static RapportAffectationDto BuildAffectations(
            IEnumerable<Domain.Entities.Affectation>  affectations,
            List<Domain.Entities.Actif>               actifs,
            Dictionary<int, Domain.Entities.Produit>  produitsParId,
            Dictionary<int, Domain.Entities.Employe>  employesParId,
            Dictionary<int, string>                   servicesParId)
        {
            var actifsParId = actifs.ToDictionary(a => a.IdActif);

            var lignes = affectations
                .Where(a => a.EstActive)
                .OrderBy(a => a.DateDebut)
                .Select(a =>
                {
                    actifsParId.TryGetValue(a.IdActif, out var actif);
                    produitsParId.TryGetValue(actif?.IdProduit ?? 0, out var produit);
                    employesParId.TryGetValue(a.IdEmploye, out var employe);

                    var nomService = employe?.IdService.HasValue == true
                        && servicesParId.TryGetValue(employe.IdService!.Value, out var ns) ? ns : "—";

                    return new RapportAffectationLigneDto
                    {
                        CodeActif  = actif?.CodeInventaire ?? $"Actif #{a.IdActif}",
                        NomProduit = produit?.NomProduit   ?? "—",
                        NomEmploye = employe != null
                            ? $"{employe.Prenom} {employe.Nom}"
                            : $"Employé #{a.IdEmploye}",
                        Service    = nomService,
                        DateDebut  = a.DateDebut,
                        DureeJours = (DateTime.Today - a.DateDebut.Date).Days
                    };
                })
                .ToList();

            return new RapportAffectationDto
            {
                TotalActives = lignes.Count,
                Affectations = lignes
            };
        }

        private static RapportMaintenanceDto BuildMaintenances(
            IEnumerable<Domain.Entities.Maintenance>  maintenances,
            List<Domain.Entities.Actif>               actifs,
            Dictionary<int, Domain.Entities.Produit>  produitsParId)
        {
            var actifsParId = actifs.ToDictionary(a => a.IdActif);

            var lignes = maintenances
                .OrderByDescending(m => m.DateMaintenance)
                .Select(m =>
                {
                    actifsParId.TryGetValue(m.IdActif, out var actif);
                    produitsParId.TryGetValue(actif?.IdProduit ?? 0, out var produit);

                    return new RapportMaintenanceLigneDto
                    {
                        CodeActif       = actif?.CodeInventaire ?? $"Actif #{m.IdActif}",
                        NomProduit      = produit?.NomProduit   ?? "—",
                        DateMaintenance = m.DateMaintenance,
                        Statut          = m.Statut.ToString(),
                        Cout            = m.Cout,
                        Description     = m.Description
                    };
                })
                .ToList();

            return new RapportMaintenanceDto
            {
                TotalMaintenances = lignes.Count,
                EnCours           = lignes.Count(l => l.Statut == "EnCours"),
                CoutTotal         = lignes.Sum(l => l.Cout),
                Maintenances      = lignes
            };
        }
    }
}
