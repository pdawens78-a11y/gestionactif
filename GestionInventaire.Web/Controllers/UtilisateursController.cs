using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UtilisateursController : Controller
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly IMapper _mapper;
        private readonly ILogger<UtilisateursController> _logger;

        public UtilisateursController(
            IUtilisateurService utilisateurService,
            IMapper mapper,
            ILogger<UtilisateursController> logger)
        {
            _utilisateurService = utilisateurService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET /Utilisateurs
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _utilisateurService.GetAllUtilisateursDtoAsync();
                var vm = _mapper.Map<UtilisateurIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement utilisateurs");
                TempData["Erreur"] = "Erreur lors du chargement des utilisateurs.";
                return View(new UtilisateurIndexViewModel());
            }
        }

        // GET /Utilisateurs/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var dto = await _utilisateurService.GetUtilisateurByIdAsync(id);
                var roles = await _utilisateurService.GetRolesDisponiblesAsync();
                var vm = _mapper.Map<UtilisateurEditViewModel>(dto);
                vm.Roles = roles.Select(r => new SelectListItem(r, r)).ToList();
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Utilisateurs/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UtilisateurEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var roles = await _utilisateurService.GetRolesDisponiblesAsync();
                vm.Roles = roles.Select(r => new SelectListItem(r, r)).ToList();
                return View(vm);
            }

            try
            {
                var dto = _mapper.Map<UtilisateurUpdateDto>(vm);
                await _utilisateurService.UpdateUtilisateurAsync(dto);

                TempData["Succes"] = $"{vm.Prenom} {vm.Nom} mis à jour avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var roles = await _utilisateurService.GetRolesDisponiblesAsync();
                vm.Roles = roles.Select(r => new SelectListItem(r, r)).ToList();
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur modification utilisateur #{Id}", vm.Id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET /Utilisateurs/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var dto = await _utilisateurService.GetUtilisateurByIdAsync(id);
                var vm = _mapper.Map<UtilisateurDeleteViewModel>(dto);
                return View(vm);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST /Utilisateurs/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _utilisateurService.SupprimerAsync(id);
                TempData["Succes"] = "Utilisateur supprimé avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur suppression utilisateur #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST /Utilisateurs/Verrouiller/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verrouiller(string id)
        {
            try
            {
                await _utilisateurService.VerrouillerAsync(id);
                TempData["Succes"] = "Compte verrouillé avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur verrouillage utilisateur #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST /Utilisateurs/Deverrouiller/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deverrouiller(string id)
        {
            try
            {
                await _utilisateurService.DeverrouillerAsync(id);
                TempData["Succes"] = "Compte déverrouillé avec succès.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erreur"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur déverrouillage utilisateur #{Id}", id);
                TempData["Erreur"] = "Une erreur inattendue est survenue.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}