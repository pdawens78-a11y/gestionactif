using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditController : Controller
    {
        private readonly IAuditService _auditService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuditController> _logger;

        public AuditController(
            IAuditService auditService,
            IMapper mapper,
            ILogger<AuditController> logger)
        {
            _auditService = auditService;
            _mapper = mapper;
            _logger = logger;
        }

        // ════════════════════════════════════════════
        // GET /Audit
        // Paramètre "Recherche" au lieu de "Action"
        // pour éviter le conflit avec RouteData["action"]
        // ════════════════════════════════════════════
        public async Task<IActionResult> Index(
            string? Query = null,
            string? Recherche = null,
            DateTime? DateDebut = null,
            DateTime? DateFin = null)
        {
            var filtre = new AuditFiltreViewModel
            {
                Query = Query,
                Action = Recherche,
                DateDebut = DateDebut,
                DateFin = DateFin
            };

            try
            {
                AuditListDto dto;

                var aFiltre = !string.IsNullOrWhiteSpace(Query)
                           || !string.IsNullOrWhiteSpace(Recherche)
                           || DateDebut.HasValue
                           || DateFin.HasValue;

                if (aFiltre)
                {
                    var filtreDto = new AuditFiltreDto
                    {
                        Query = Query,
                        Action = Recherche,
                        DateDebut = DateDebut,
                        DateFin = DateFin
                    };
                    dto = await _auditService.RechercherAsync(filtreDto);
                }
                else
                {
                    dto = await _auditService.GetLogsAsync();
                }

                var vm = _mapper.Map<AuditIndexViewModel>(dto);
                vm.Filtre = filtre;
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement audit");
                TempData["Erreur"] = "Erreur lors du chargement du journal d'audit.";
                return View(new AuditIndexViewModel
                {
                    Filtre = filtre
                });
            }
        }
    }
}