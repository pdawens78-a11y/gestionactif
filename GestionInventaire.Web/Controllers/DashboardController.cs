using AutoMapper;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            IMapper mapper,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _dashboardService.GetDashboardDtoAsync();
                var vm = _mapper.Map<DashboardViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du dashboard");
                TempData["Erreur"] = "Une erreur est survenue lors du chargement du tableau de bord.";
                return View(new DashboardViewModel());
            }
        }
    }
}