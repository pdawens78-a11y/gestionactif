using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize(Roles = "Admin,Gestionnaire")]
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IMapper _mapper;
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(
            IServiceService serviceService,
            IMapper mapper,
            ILogger<ServicesController> logger)
        {
            _serviceService = serviceService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET /Services
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _serviceService.GetAllServicesDtoAsync();
                var vm = _mapper.Map<ServiceIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement services");
                TempData["Erreur"] = "Erreur lors du chargement des services.";
                return View(new ServiceIndexViewModel());
            }
        }

        // GET /Services/Create
        public IActionResult Create() => View(new ServiceCreateViewModel());

        // POST /Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<ServiceCreateDto>(vm);
                await _serviceService.CreateServiceAsync(dto);

                TempData["Succes"] = $"Service « {vm.NomService} » créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur création service");
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Services/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var detail = await _serviceService.GetServiceByIdAsync(id);
                var vm = _mapper.Map<ServiceEditViewModel>(detail);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Services/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var dto = _mapper.Map<ServiceEditDto>(vm);
                await _serviceService.UpdateServiceAsync(dto);

                TempData["Succes"] = $"Service « {vm.NomService} » modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification service #{Id}", vm.IdService);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Services/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var detail = await _serviceService.GetServiceByIdAsync(id);
                var dto = await _serviceService.GetAllServicesDtoAsync();
                var item = dto.Services.FirstOrDefault(s => s.IdService == id);

                var vm = _mapper.Map<ServiceDeleteViewModel>(detail);
                vm.NombreEmployes = item?.NombreEmployes ?? 0;
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Services/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _serviceService.DeleteServiceAsync(id);
                TempData["Succes"] = "Service supprimé avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur suppression service #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}