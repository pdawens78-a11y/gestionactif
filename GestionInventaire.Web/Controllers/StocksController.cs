using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Stocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize(Roles = "Admin,Gestionnaire")]
    public class StocksController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly ILogger<StocksController> _logger;

        public StocksController(
            IStockService stockService,
            IMapper mapper,
            ILogger<StocksController> logger)
        {
            _stockService = stockService;
            _mapper = mapper;
            _logger = logger;
        }

        // ════════════════════════════════════════════
        // GET /Stocks
        // ════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _stockService.GetAllStocksDtoAsync();
                var vm = _mapper.Map<StockIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement stocks");
                TempData["Erreur"] = "Erreur lors du chargement des stocks.";
                return View(new StockIndexViewModel());
            }
        }

        // ════════════════════════════════════════════
        // GET /Stocks/Edit/{id}
        // ════════════════════════════════════════════
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                TempData["Erreur"] = "ID de stock invalide.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = await _stockService.GetStockByIdAsync(id);
                var vm = _mapper.Map<StockEditViewModel>(dto);

                if (vm == null)
                {
                    TempData["Erreur"] = "Le stock demandé n'existe pas.";
                    return RedirectToAction(nameof(Index));
                }

                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement stock #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // POST /Stocks/Edit/{id}
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StockEditViewModel vm)
        {
            if (id <= 0 || vm.IdStock != id)
            {
                TempData["Erreur"] = "Données invalides.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalide pour stock #{Id}", id);
                return View(vm);
            }

            try
            {
                var dto = _mapper.Map<StockUpdateDto>(vm);
                await _stockService.UpdateStockAsync(dto);

                TempData["Succes"] = $"Stock de « {vm.NomProduit} » mis à jour avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Stock #{Id} introuvable", id);
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification stock #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // GET /Stocks/Mouvement/{id}
        // ════════════════════════════════════════════
        public async Task<IActionResult> Mouvement(int id)
        {
            try
            {
                var dto = await _stockService.GetStockByIdAsync(id);
                var vm = new StockMouvementViewModel
                {
                    IdStock = dto.IdStock,
                    NomProduit = dto.NomProduit,
                    QuantiteActuelle = dto.Quantite
                };
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // POST /Stocks/Mouvement
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Mouvement(StockMouvementViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<StockMouvementDto>(vm);
                await _stockService.AjouterMouvementAsync(dto);

                var label = vm.Type == "Entree" ? "Entrée" : "Sortie";
                TempData["Succes"] = $"{label} de {vm.Quantite} unité(s) enregistrée pour « {vm.NomProduit} ».";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur mouvement stock #{Id}", vm.IdStock);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // GET /Stocks/Historique/{id}
        // ════════════════════════════════════════════
        public async Task<IActionResult> Historique(int id)
        {
            try
            {
                var dto = await _stockService.GetHistoriqueAsync(id);
                var vm = _mapper.Map<StockHistoriqueViewModel>(dto);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}