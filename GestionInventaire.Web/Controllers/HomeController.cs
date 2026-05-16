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
                var currentUserId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

                var userRoles = User.Claims
                    .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToArray();

                var dto = await _homeService.GetHomeDtoAsync(currentUserId, userRoles);
                var vm = _mapper.Map<HomeViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de la page d'accueil");
                TempData["Erreur"] = "Une erreur est survenue lors du chargement de la page d'accueil.";
                return View(new HomeViewModel());
            }
        }
    }
}