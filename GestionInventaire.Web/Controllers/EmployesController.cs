using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Employes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class EmployesController : Controller
    {
        private readonly IEmployeService _employeService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployesController> _logger;

        public EmployesController(
            IEmployeService employeService,
            IMapper mapper,
            ILogger<EmployesController> logger)
        {
            _employeService = employeService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET /Employes
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _employeService.GetAllEmployesDtoAsync();
                var vm = _mapper.Map<EmployeIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement employés");
                TempData["Erreur"] = "Erreur lors du chargement des employés.";
                return View(new EmployeIndexViewModel());
            }
        }

        // GET /Employes/Create
        public IActionResult Create()
            => View(new EmployeCreateViewModel());

        // POST /Employes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<EmployeCreateDto>(vm);
                await _employeService.CreateEmployeAsync(dto);

                TempData["Succes"] = $"{vm.Prenom} {vm.Nom} ajouté(e) avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur création employé");
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Employes/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto = await _employeService.GetEmployeByIdAsync(id);
                var vm = _mapper.Map<EmployeEditViewModel>(dto);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Employes/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<EmployeEditDto>(vm);
                await _employeService.UpdateEmployeAsync(dto);

                TempData["Succes"] = $"{vm.Prenom} {vm.Nom} modifié(e) avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification employé #{Id}", vm.IdEmploye);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Employes/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dto = await _employeService.GetEmployeByIdAsync(id);
                var vm = _mapper.Map<EmployeDeleteViewModel>(dto);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Employes/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _employeService.DeleteEmployeAsync(id);
                TempData["Succes"] = "Employé supprimé avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur suppression employé #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}