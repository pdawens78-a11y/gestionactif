using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Actifs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class ActifsController : Controller
    {
        private readonly IActifService _actifService;
        private readonly IProduitService _produitService;
        private readonly IMapper _mapper;
        private readonly ILogger<ActifsController> _logger;

        public ActifsController(
            IActifService actifService,
            IProduitService produitService,
            IMapper mapper,
            ILogger<ActifsController> logger)
        {
            _actifService = actifService;
            _produitService = produitService;
            _mapper = mapper;
            _logger = logger;
        }

        // ════════════════════════════════════════════
        // GET /Actifs
        // GET /Actifs?filtreStatut=Disponible
        // ════════════════════════════════════════════
        public async Task<IActionResult> Index(string? filtreStatut)
        {
            try
            {
                var dto = await _actifService.GetAllActifsDtoAsync();
                var vm = _mapper.Map<ActifIndexViewModel>(dto);
                vm.FiltreStatut = filtreStatut;

                // Appliquer le filtre côté ViewModel
                if (!string.IsNullOrWhiteSpace(filtreStatut))
                {
                    vm.Actifs = vm.Actifs
                        .Where(a => a.Statut == filtreStatut)
                        .ToList();
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement actifs");
                TempData["Erreur"] = "Erreur lors du chargement des actifs.";
                return View(new ActifIndexViewModel());
            }
        }

        // ════════════════════════════════════════════
        // GET /Actifs/Edit/{id}
        // ════════════════════════════════════════════
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto = await _actifService.GetActifEditDtoAsync(id);
                var vm = _mapper.Map<ActifEditViewModel>(dto);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // POST /Actifs/Edit/{id}
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActifEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<ActifUpdateDto>(vm);
                await _actifService.UpdateActifDtoAsync(dto);

                TempData["Succes"] = $"Actif {vm.CodeInventaire} mis à jour avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification actif #{Id}", vm.IdActif);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // GET /Actifs/Approvisionner/{idProduit}
        // ════════════════════════════════════════════
        public async Task<IActionResult> Approvisionner(int idProduit)
        {
            try
            {
                // Réutilise IProduitService pour récupérer les infos du produit
                var produitDetail = await _produitService.GetProduitByIdAsync(idProduit);

                var vm = new ApprovisionnerViewModel
                {
                    IdProduit = produitDetail.IdProduit,
                    NomProduit = produitDetail.NomProduit,
                    NomCategorie = produitDetail.NomCategorie,
                    StockActuel = 0, // sera enrichi si besoin
                    Quantite = 1
                };

                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction("Index", "Produits");
            }
        }

        // ════════════════════════════════════════════
        // POST /Actifs/Approvisionner
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approvisionner(ApprovisionnerViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<ApprovisionnerDto>(vm);
                var result = await _actifService.ApprovisionnerAsync(dto);
                var vmResult = _mapper.Map<ApprovisionnerResultViewModel>(result);

                TempData["Succes"] =
                    $"{result.NombreGenere} actif(s) générés avec succès pour « {result.NomProduit} ».";

                // Passer le résultat via TempData pour la vue de confirmation
                return View("ApprovisionnerResult", vmResult);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur approvisionnement produit #{Id}", vm.IdProduit);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction("Index", "Produits");
            }
        }
    }
}