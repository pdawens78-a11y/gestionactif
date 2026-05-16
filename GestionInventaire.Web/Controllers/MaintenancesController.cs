using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Maintenances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class MaintenancesController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IMapper _mapper;
        private readonly ILogger<MaintenancesController> _logger;

        public MaintenancesController(
            IMaintenanceService maintenanceService,
            IMapper mapper,
            ILogger<MaintenancesController> logger)
        {
            _maintenanceService = maintenanceService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET /Maintenances
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _maintenanceService.GetAllMaintenancesDtoAsync();
                var vm = _mapper.Map<MaintenanceIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement maintenances");
                TempData["Erreur"] = "Erreur lors du chargement des maintenances.";
                return View(new MaintenanceIndexViewModel());
            }
        }

        // GET /Maintenances/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var actifs = await _maintenanceService.GetActifsForMaintenanceAsync();
                var vm = new MaintenanceCreateViewModel
                {
                    DateMaintenance = DateTime.Today,
                    Actifs = _mapper.Map<List<SelectListItem>>(actifs)
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement formulaire maintenance");
                TempData["Erreur"] = "Erreur lors du chargement du formulaire.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Maintenances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var actifs = await _maintenanceService.GetActifsForMaintenanceAsync();
                vm.Actifs = _mapper.Map<List<SelectListItem>>(actifs);
                return View(vm);
            }

            try
            {
                var dto = _mapper.Map<MaintenanceCreateDto>(vm);
                await _maintenanceService.CreateMaintenanceAsync(dto);

                TempData["Succes"] = "Maintenance planifiée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var actifs = await _maintenanceService.GetActifsForMaintenanceAsync();
                vm.Actifs = _mapper.Map<List<SelectListItem>>(actifs);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur création maintenance");
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Maintenances/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var detail = await _maintenanceService.GetMaintenanceByIdAsync(id);
                var vm = _mapper.Map<MaintenanceEditViewModel>(detail);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Maintenances/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaintenanceEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<MaintenanceEditDto>(vm);
                await _maintenanceService.UpdateMaintenanceAsync(dto);

                TempData["Succes"] = "Maintenance mise à jour avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification maintenance #{Id}", vm.IdMaintenance);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Maintenances/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var detail = await _maintenanceService.GetMaintenanceByIdAsync(id);
                var vm = _mapper.Map<MaintenanceDeleteViewModel>(detail);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Maintenances/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _maintenanceService.DeleteMaintenanceAsync(id);
                TempData["Succes"] = "Maintenance supprimée avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur suppression maintenance #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST /Maintenances/ChangerStatut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangerStatut(int id, string statut)
        {
            try
            {
                await _maintenanceService.ChangerStatutAsync(id, statut);
                TempData["Succes"] = $"Statut mis à jour : {statut}.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur changement statut maintenance #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}