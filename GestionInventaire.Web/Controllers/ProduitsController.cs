using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Produits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Controllers
{
    [Authorize(Roles = "Admin,Gestionnaire")]
    public class ProduitsController : Controller
    {
        private readonly IProduitService _produitService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProduitsController> _logger;

        public ProduitsController(
            IProduitService produitService,
            IMapper mapper,
            ILogger<ProduitsController> logger)
        {
            _produitService = produitService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET /Produits
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _produitService.GetAllProduitsDtoAsync();
                var vm = _mapper.Map<ProduitIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement produits");
                TempData["Erreur"] = "Erreur lors du chargement des produits.";
                return View(new ProduitIndexViewModel());
            }
        }

        // GET /Produits/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var formData = await _produitService.GetFormDataAsync();
                var vm = new ProduitCreateViewModel
                {
                    Categories = _mapper.Map<List<SelectListItem>>(formData.Categories)
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement formulaire produit");
                TempData["Erreur"] = "Erreur lors du chargement du formulaire.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Produits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProduitCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var formData = await _produitService.GetFormDataAsync();
                vm.Categories = _mapper.Map<List<SelectListItem>>(formData.Categories);
                return View(vm);
            }

            try
            {
                var dto = _mapper.Map<ProduitCreateDto>(vm);
                await _produitService.CreateProduitAsync(dto);
                TempData["Succes"] = $"Produit « {vm.NomProduit} » créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var formData = await _produitService.GetFormDataAsync();
                vm.Categories = _mapper.Map<List<SelectListItem>>(formData.Categories);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur création produit");
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Produits/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var detail = await _produitService.GetProduitByIdAsync(id);
                var formData = await _produitService.GetFormDataAsync();
                var vm = _mapper.Map<ProduitEditViewModel>(detail);
                vm.Categories = _mapper.Map<List<SelectListItem>>(formData.Categories);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Produits/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProduitEditViewModel vm, string? redirectApres)
        {
            if (!ModelState.IsValid)
            {
                var formData = await _produitService.GetFormDataAsync();
                vm.Categories = _mapper.Map<List<SelectListItem>>(formData.Categories);
                return View(vm);
            }

            try
            {
                var dto = _mapper.Map<ProduitEditDto>(vm);
                await _produitService.UpdateProduitAsync(dto);

                // ── Bouton "Approvisionner" cliqué ──
                if (redirectApres == "approvisionner")
                {
                    // Calculer la quantité manquante = stock - actifs existants
                    var detail = await _produitService.GetProduitByIdAsync(vm.IdProduit);
                    int stockCible = vm.StockQuantite;
                    int actifsExistants = detail.NombreActifs;
                    int quantiteManquante = stockCible - actifsExistants;

                    if (quantiteManquante <= 0)
                    {
                        // Stock déjà cohérent avec les actifs
                        TempData["Succes"] = $"Produit « {vm.NomProduit} » sauvegardé. " +
                                             $"Le nombre d'actifs ({actifsExistants}) correspond déjà au stock ({stockCible}).";
                        return RedirectToAction(nameof(Index));
                    }

                    // Passer la quantité manquante via TempData
                    TempData["Succes"] = $"Produit « {vm.NomProduit} » sauvegardé.";
                    TempData["ApproquantiteAuto"] = quantiteManquante;

                    return RedirectToAction("Approvisionner", "Actifs",
                        new { idProduit = vm.IdProduit, quantiteAuto = quantiteManquante });
                }

                TempData["Succes"] = $"Produit « {vm.NomProduit} » modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var formData = await _produitService.GetFormDataAsync();
                vm.Categories = _mapper.Map<List<SelectListItem>>(formData.Categories);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification produit #{Id}", vm.IdProduit);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Produits/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var detail = await _produitService.GetProduitByIdAsync(id);
                var vm = _mapper.Map<ProduitDeleteViewModel>(detail);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Produits/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _produitService.DeleteProduitAsync(id);
                TempData["Succes"] = "Produit supprimé avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur suppression produit #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}