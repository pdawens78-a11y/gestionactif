using GestionInventaire.BLL.Services;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;
using GestionInventaire.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IActifService _actifService;
        private readonly IStockRepository _stockRepository;
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly IProduitRepository _produitRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IActifService actifService,
            IStockRepository stockRepository,
            IMaintenanceRepository maintenanceRepository,
            IAuditRepository auditRepository,
            IProduitRepository produitRepository,
            IUtilisateurRepository utilisateurRepository,
            ILogger<DashboardController> logger)
        {
            _actifService = actifService;
            _stockRepository = stockRepository;
            _maintenanceRepository = maintenanceRepository;
            _auditRepository = auditRepository;
            _produitRepository = produitRepository;
            _utilisateurRepository = utilisateurRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Si l'utilisateur n'est pas authentifié, le rediriger vers le login
            //if (!User.Identity?.IsAuthenticated ?? true)
            //{
            //    return Redirect("/Identity/Account/Login");
            //}

            try
            {
                var actifs = (await _actifService.GetAllActifsAsync()).ToList();
                var stocks = await _stockRepository.GetAllAsync();
                var maintenances = await _maintenanceRepository.GetAllAsync();
                var audits = _auditRepository.GetLogs();
                var produits = await _produitRepository.GetAllAsync();
                var utilisateurs = await _utilisateurRepository.GetAllAsync();

                var produitsParId = produits.ToDictionary(p => p.IdProduit, p => p.NomProduit);
                var actifsParId = actifs.ToDictionary(a => a.IdActif, a => a.CodeInventaire);
                var utilisateursParId = utilisateurs.ToDictionary(u => u.Id, u => u.NomComplet);

                var vm = new DashboardViewModel
                {
                    TotalActifs = actifs.Count,
                    ActifsDisponibles = actifs.Count(a => a.Statut == StatutActif.Disponible),
                    ActifsAffectes = actifs.Count(a => a.Statut == StatutActif.Affecte),
                    ActifsEnMaintenance = actifs.Count(a => a.Statut == StatutActif.EnMaintenance),
                    ActifsHorsService = actifs.Count(a => a.Statut == StatutActif.HorsService),
                    StockCritique = stocks.Count(s => s.Quantite <= s.SeuilAlerte),

                    AlertesStock = stocks
                        .Where(s => s.Quantite <= s.SeuilAlerte)
                        .OrderBy(s => s.Quantite)
                        .Select(s => new AlerteStockViewModel
                        {
                            NomActif = produitsParId.TryGetValue(s.IdProduit, out var nomProduit)
                                ? nomProduit
                                : $"Produit #{s.IdProduit}",
                            Quantite = s.Quantite
                        })
                        .ToList(),

                    AlertesMaintenance = maintenances
                        .Where(m => m.Statut != StatutMaintenance.Terminee &&
                                    m.DateMaintenance <= DateTime.Now.AddDays(3))
                        .OrderBy(m => m.DateMaintenance)
                        .Select(m => new AlerteMaintenanceViewModel
                        {
                            NomActif = actifsParId.TryGetValue(m.IdActif, out var codeInventaire)
                                ? codeInventaire
                                : $"Actif #{m.IdActif}",
                            DateMaintenance = m.DateMaintenance
                        })
                        .ToList(),

                    DerniersAudits = audits
                        .OrderByDescending(a => a.DateAction)
                        .Take(10)
                        .Select(a => new AuditItemViewModel
                        {
                            Action = a.Action,
                            UtilisateurNom = utilisateursParId.TryGetValue(a.IdUtilisateur, out var nomComplet)
                                ? nomComplet
                                : a.IdUtilisateur,
                            DateAction = a.DateAction
                        })
                        .ToList()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du dashboard");
                TempData["Erreur"] = "Une erreur est survenue lors du chargement du tableau de bord.";
                return View(new DashboardViewModel());
            }
        }
    }
}