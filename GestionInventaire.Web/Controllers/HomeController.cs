using AutoMapper;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IHomeService homeService,
            IMapper mapper,
            ILogger<HomeController> logger)
        {
            _homeService = homeService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _homeService.GetHomeDtoAsync();
                var vm = _mapper.Map<HomeViewModel>(dto);

                // ── Récupérer le vrai prénom de l'utilisateur connecté ──
                var userPrenom = User?.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value;
                var userNom = User?.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;
                var userEmail = User?.Identity?.Name ?? "Utilisateur";

                // ── Si pas de prénom/nom, utiliser l'email ou "Utilisateur" ──
                if (!string.IsNullOrEmpty(userPrenom))
                {
                    vm.NomUtilisateur = userPrenom;
                }
                else if (!string.IsNullOrEmpty(userNom))
                {
                    vm.NomUtilisateur = userNom;
                }
                else
                {
                    vm.NomUtilisateur = userEmail.Split('@')[0]; // Avant le @
                }

                // ── Récupérer le rôle ──
                vm.RoleUtilisateur = User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "Utilisateur";
                
                // ── Date de connexion ──
                vm.DateConnexion = DateTime.Now;

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement page d'accueil");
                return View(new HomeViewModel());
            }
        }
    }
}