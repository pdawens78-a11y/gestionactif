using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IActifService          _actifService;
        private readonly IStockRepository       _stockRepository;
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IAuditRepository       _auditRepository;
        private readonly IProduitRepository     _produitRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;

        public DashboardService(
            IActifService          actifService,
            IStockRepository       stockRepository,
            IMaintenanceRepository maintenanceRepository,
            IAuditRepository       auditRepository,
            IProduitRepository     produitRepository,
            IUtilisateurRepository utilisateurRepository)
        {
            _actifService          = actifService;
            _stockRepository       = stockRepository;
            _maintenanceRepository = maintenanceRepository;
            _auditRepository       = auditRepository;
            _produitRepository     = produitRepository;
            _utilisateurRepository = utilisateurRepository;
        }

        public async Task<DashboardDto> GetDashboardDtoAsync()
        {
            // ── Séquentiel — EF Core interdit Task.WhenAll sur le même DbContext ──
            var actifs       = (await _actifService.GetAllActifsAsync()).ToList();
            var stocks       = await _stockRepository.GetAllAsync();
            var maintenances = await _maintenanceRepository.GetAllAsync();
            var audits       = await _auditRepository.GetLogsAsync();
            var produits     = await _produitRepository.GetAllAsync();
            var utilisateurs = await _utilisateurRepository.GetAllAsync();

            var produitsParId     = produits.ToDictionary(p => p.IdProduit, p => p.NomProduit);
            var actifsParId       = actifs.ToDictionary(a => a.IdActif, a => a.CodeInventaire);
            var utilisateursParId = utilisateurs.ToDictionary(u => u.Id, u =>
            {
                var nom = u.NomComplet?.Trim();
                if (!string.IsNullOrEmpty(nom)) return nom;
                if (!string.IsNullOrEmpty(u.Email)) return u.Email;
                return u.UserName ?? u.Id[..8];
            });

            return new DashboardDto
            {
                TotalActifs         = actifs.Count,
                ActifsDisponibles   = actifs.Count(a => a.Statut == StatutActif.Disponible),
                ActifsAffectes      = actifs.Count(a => a.Statut == StatutActif.Affecte),
                ActifsEnMaintenance = actifs.Count(a => a.Statut == StatutActif.EnMaintenance),
                ActifsHorsService   = actifs.Count(a => a.Statut == StatutActif.HorsService),
                StockCritique       = stocks.Count(s => s.Quantite <= s.SeuilAlerte),

                AlertesStock = stocks
                    .Where(s => s.Quantite <= s.SeuilAlerte)
                    .OrderBy(s => s.Quantite)
                    .Select(s => new AlerteStockDto
                    {
                        NomProduit = produitsParId.TryGetValue(s.IdProduit, out var np)
                            ? np : $"Produit #{s.IdProduit}",
                        Quantite = s.Quantite
                    })
                    .ToList(),

                AlertesMaintenance = maintenances
                    .Where(m => m.Statut != StatutMaintenance.Terminee
                             && m.DateMaintenance <= DateTime.Now.AddDays(3))
                    .OrderBy(m => m.DateMaintenance)
                    .Select(m => new AlerteMaintenanceDto
                    {
                        NomActif = actifsParId.TryGetValue(m.IdActif, out var code)
                            ? code : $"Actif #{m.IdActif}",
                        DateMaintenance = m.DateMaintenance,
                        EstUrgente      = m.DateMaintenance <= DateTime.Now.AddDays(1)
                    })
                    .ToList(),

                DerniersAudits = audits
                    .OrderByDescending(a => a.DateAction)
                    .Take(10)
                    .Select(a => new AuditItemDto
                    {
                        Action         = a.Action,
                        UtilisateurNom = utilisateursParId.TryGetValue(
                                             a.IdUtilisateur, out var nom)
                                         ? nom : a.IdUtilisateur,
                        DateAction = a.DateAction
                    })
                    .ToList()
            };
        }
    }
}
