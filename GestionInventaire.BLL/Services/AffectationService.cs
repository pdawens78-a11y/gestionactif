using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class AffectationService : IAffectationService
    {
        private readonly IAffectationRepository   _affectationRepository;
        private readonly IActifService            _actifService;
        private readonly IActifRepository         _actifRepository;
        private readonly IEmployeRepository       _employeRepository;
        private readonly IServiceRepository       _serviceRepository;
        private readonly IStockRepository         _stockRepository;
        private readonly IMouvementStockRepository _mouvementStockRepository;
        private readonly IProduitRepository       _produitRepository;
        private readonly IAuditRepository         _auditRepository;

        public AffectationService(
            IAffectationRepository    affectationRepository,
            IActifService             actifService,
            IActifRepository          actifRepository,
            IEmployeRepository        employeRepository,
            IServiceRepository        serviceRepository,
            IStockRepository          stockRepository,
            IMouvementStockRepository mouvementStockRepository,
            IProduitRepository        produitRepository,
            IAuditRepository          auditRepository)
        {
            _affectationRepository    = affectationRepository;
            _actifService             = actifService;
            _actifRepository          = actifRepository;
            _employeRepository        = employeRepository;
            _serviceRepository        = serviceRepository;
            _stockRepository          = stockRepository;
            _mouvementStockRepository = mouvementStockRepository;
            _produitRepository        = produitRepository;
            _auditRepository          = auditRepository;
        }

        public async Task<AffectationListDto> GetAllAffectationsDtoAsync()
        {
            var affectations = await _affectationRepository.GetAllAsync();
            var actifs       = (await _actifService.GetAllActifsAsync()).ToList();
            var employes     = await _employeRepository.GetAllAsync();
            var produits     = await _produitRepository.GetAllAsync();
            var services     = await _serviceRepository.GetAllAsync();

            var actifsParId   = actifs.ToDictionary(a => a.IdActif);
            var employesParId = employes.ToDictionary(e => e.IdEmploye);
            var produitsParId = produits.ToDictionary(p => p.IdProduit, p => p.NomProduit);
            var servicesParId = services.ToDictionary(s => s.IdService, s => s.NomService);

            var items = affectations
                .OrderByDescending(a => a.DateDebut)
                .Select(a =>
                {
                    actifsParId.TryGetValue(a.IdActif, out var actif);
                    employesParId.TryGetValue(a.IdEmploye, out var employe);

                    var nomProduit = actif != null
                        && produitsParId.TryGetValue(actif.IdProduit, out var np)
                        ? np : "—";

                    var nomService = employe?.IdService.HasValue == true
                        && servicesParId.TryGetValue(employe.IdService!.Value, out var ns)
                        ? ns : "—";

                    return new AffectationItemDto
                    {
                        IdAffectation  = a.IdAffectation,
                        CodeActif      = actif?.CodeInventaire ?? $"Actif #{a.IdActif}",
                        NomProduit     = nomProduit,
                        NomEmploye     = employe != null
                            ? $"{employe.Prenom} {employe.Nom}"
                            : $"Employé #{a.IdEmploye}",
                        ServiceEmploye = nomService,
                        DateDebut      = a.DateDebut,
                        DateFin        = a.DateFin,
                        EstActive      = a.EstActive
                    };
                })
                .ToList();

            return new AffectationListDto
            {
                Affectations   = items,
                TotalActives   = items.Count(i => i.EstActive),
                TotalTerminees = items.Count(i => !i.EstActive)
            };
        }

        public async Task<AffectationFormDataDto> GetFormDataAsync()
        {
            var actifs    = (await _actifService.GetAllActifsAsync()).ToList();
            var employes  = await _employeRepository.GetAllAsync();
            var produits  = await _produitRepository.GetAllAsync();
            var services  = await _serviceRepository.GetAllAsync();

            var produitsParId = produits.ToDictionary(p => p.IdProduit, p => p.NomProduit);
            var servicesParId = services.ToDictionary(s => s.IdService, s => s.NomService);

            var actifsDisponibles = actifs
                .Where(a => a.Statut == StatutActif.Disponible)
                .Select(a => new ActifDisponibleDto
                {
                    IdActif        = a.IdActif,
                    CodeInventaire = a.CodeInventaire,
                    NomProduit     = produitsParId.TryGetValue(a.IdProduit, out var n) ? n : "—",
                    Localisation   = a.Localisation
                })
                .ToList();

            var employesSelect = employes
                .OrderBy(e => e.Nom)
                .Select(e => new EmployeSelectDto
                {
                    IdEmploye  = e.IdEmploye,
                    NomComplet = $"{e.Prenom} {e.Nom}",
                    Service    = e.IdService.HasValue
                        && servicesParId.TryGetValue(e.IdService.Value, out var ns)
                        ? ns : "—"
                })
                .ToList();

            return new AffectationFormDataDto
            {
                ActifsDisponibles = actifsDisponibles,
                Employes          = employesSelect
            };
        }

        public async Task AffecterAsync(AffectationCreateDto dto, string currentUserId)
        {
            var actif = await _actifService.GetActifByIdAsync(dto.IdActif);

            if (actif.Statut != StatutActif.Disponible)
                throw new InvalidOperationException(
                    $"L'actif {actif.CodeInventaire} n'est pas disponible (statut : {actif.Statut}).");

            var employe = await _employeRepository.GetByIdAsync(dto.IdEmploye)
                ?? throw new InvalidOperationException($"L'employé #{dto.IdEmploye} n'existe pas.");

            var affectation = new Affectation
            {
                IdActif   = dto.IdActif,
                IdEmploye = dto.IdEmploye,
                DateDebut = dto.DateDebut
            };

            await _affectationRepository.CreateAsync(affectation);

            actif.Statut = StatutActif.Affecte;
            _actifRepository.Update(actif);

            var stocks = await _stockRepository.GetAllAsync();
            var stock  = stocks.FirstOrDefault(s => s.IdProduit == actif.IdProduit);

            if (stock != null && stock.Quantite > 0)
            {
                stock.Quantite -= 1;
                _stockRepository.Update(stock);

                await _mouvementStockRepository.CreateAsync(new MouvementStock
                {
                    IdStock       = stock.IdStock,
                    Type          = TypeMouvement.Sortie,
                    Quantite      = 1,
                    DateMouvement = DateTime.Now
                });
            }

            await _affectationRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Affectation : {actif.CodeInventaire} → {employe.Prenom} {employe.Nom}",
                "Affectation", affectation.IdAffectation);
        }

        public async Task RetournerAsync(int idAffectation, string currentUserId)
        {
            var affectation = await _affectationRepository.GetByIdAsync(idAffectation)
                ?? throw new InvalidOperationException($"L'affectation #{idAffectation} n'existe pas.");

            if (!affectation.EstActive)
                throw new InvalidOperationException("Cette affectation est déjà clôturée.");

            affectation.DateFin = DateTime.Now;
            _affectationRepository.Update(affectation);

            var actif = await _actifService.GetActifByIdAsync(affectation.IdActif);
            actif.Statut = StatutActif.Disponible;
            _actifRepository.Update(actif);

            var stocks = await _stockRepository.GetAllAsync();
            var stock  = stocks.FirstOrDefault(s => s.IdProduit == actif.IdProduit);

            if (stock != null)
            {
                stock.Quantite += 1;
                _stockRepository.Update(stock);

                await _mouvementStockRepository.CreateAsync(new MouvementStock
                {
                    IdStock       = stock.IdStock,
                    Type          = TypeMouvement.Entree,
                    Quantite      = 1,
                    DateMouvement = DateTime.Now
                });
            }

            await _affectationRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Retour actif : {actif.CodeInventaire} (affectation #{idAffectation})",
                "Affectation", idAffectation);
        }
    }
}
