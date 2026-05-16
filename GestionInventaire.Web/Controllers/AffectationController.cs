using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Affectations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class AffectationsController : Controller
    {
        private readonly IAffectationService _affectationService;
        private readonly IMapper _mapper;
        private readonly ILogger<AffectationsController> _logger;

        public AffectationsController(
            IAffectationService affectationService,
            IMapper mapper,
            ILogger<AffectationsController> logger)
        {
            _affectationService = affectationService;
            _mapper = mapper;
            _logger = logger;
        }

        // ════════════════════════════════════════════
        // GET /Affectations
        // ════════════════════════════════════════════
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _affectationService.GetAllAffectationsDtoAsync();
                var vm = _mapper.Map<AffectationIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement affectations");
                TempData["Erreur"] = "Erreur lors du chargement des affectations.";
                return View(new AffectationIndexViewModel());
            }
        }

        // ════════════════════════════════════════════
        // GET /Affectations/Create
        // ════════════════════════════════════════════
        public async Task<IActionResult> Create()
        {
            try
            {
                var formData = await _affectationService.GetFormDataAsync();
                var vm = BuildCreateViewModel(formData);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement formulaire affectation");
                TempData["Erreur"] = "Erreur lors du chargement du formulaire.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // POST /Affectations/Create
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AffectationCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var formData = await _affectationService.GetFormDataAsync();
                vm.ActifsDisponibles = _mapper.Map<List<SelectListItem>>(formData.ActifsDisponibles);
                vm.Employes = _mapper.Map<List<SelectListItem>>(formData.Employes);
                return View(vm);
            }

            try
            {
                var currentUserId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

                var dto = _mapper.Map<AffectationCreateDto>(vm);
                await _affectationService.AffecterAsync(dto, currentUserId);

                TempData["Succes"] = "Affectation créée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var formData = await _affectationService.GetFormDataAsync();
                vm.ActifsDisponibles = _mapper.Map<List<SelectListItem>>(formData.ActifsDisponibles);
                vm.Employes = _mapper.Map<List<SelectListItem>>(formData.Employes);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur création affectation");
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ════════════════════════════════════════════
        // POST /Affectations/Retourner/{id}
        // ════════════════════════════════════════════
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retourner(int id)
        {
            try
            {
                var currentUserId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

                await _affectationService.RetournerAsync(id, currentUserId);
                TempData["Succes"] = "Actif retourné avec succès. Statut remis à Disponible.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur retour affectation #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ════════════════════════════════════════════
        // HELPER PRIVÉ
        // ════════════════════════════════════════════

        private AffectationCreateViewModel BuildCreateViewModel(AffectationFormDataDto formData)
        {
            return new AffectationCreateViewModel
            {
                DateDebut = DateTime.Today,
                ActifsDisponibles = _mapper.Map<List<SelectListItem>>(formData.ActifsDisponibles),
                Employes = _mapper.Map<List<SelectListItem>>(formData.Employes)
            };
        }
    }
}