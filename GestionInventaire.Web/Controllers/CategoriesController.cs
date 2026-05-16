using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategorieService _categorieService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ICategorieService categorieService,
            IMapper mapper,
            ILogger<CategoriesController> logger)
        {
            _categorieService = categorieService;
            _mapper = mapper;
            _logger = logger;
        }

        // ════════════════════════════════════════════
        // GET /Categories
        // ════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _categorieService.GetAllCategoriesDtoAsync();
                var vm = _mapper.Map<CategorieIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement catégories");
                TempData["Erreur"] = "Erreur lors du chargement des catégories.";
                return View(new CategorieIndexViewModel());
            }
        }

        // ════════════════════════════════════════════
        // GET /Categories/Create
        // ════════════════════════════════════════════
        public IActionResult Create()
            => View(new CategorieCreateViewModel());

        // ════════════════════════════════════════════
        // POST /Categories/Create
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategorieCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<CategorieCreateDto>(vm);
                await _categorieService.CreateCategorieAsync(dto);

                TempData["Succes"] = $"Catégorie « {vm.NomCategorie} » créée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur création catégorie");
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // GET /Categories/Edit/{id}
        // ════════════════════════════════════════════
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto = await _categorieService.GetCategorieByIdAsync(id);
                var vm = _mapper.Map<CategorieEditViewModel>(dto);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // POST /Categories/Edit/{id}
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategorieEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<CategorieEditDto>(vm);
                await _categorieService.UpdateCategorieAsync(dto);

                TempData["Succes"] = $"Catégorie « {vm.NomCategorie} » modifiée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification catégorie #{Id}", vm.IdCategorie);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // GET /Categories/Delete/{id}
        // ════════════════════════════════════════════
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dto = await _categorieService.GetCategorieByIdAsync(id);
                var vm = _mapper.Map<CategorieDeleteViewModel>(dto);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // POST /Categories/Delete/{id}
        // ════════════════════════════════════════════
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categorieService.DeleteCategorieAsync(id);
                TempData["Succes"] = "Catégorie supprimée avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur suppression catégorie #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}