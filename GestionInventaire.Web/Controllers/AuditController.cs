using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
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
        // GET /Audit?Query=...&Action=...&DateDebut=...&DateFin=...
        // ════════════════════════════════════════════
        public async Task<IActionResult> Index(AuditFiltreViewModel? filtre)
        {
            // Garantir que filtre n'est jamais null
            filtre ??= new AuditFiltreViewModel();

            try
            {
                AuditListDto dto;

                var aFiltre = !string.IsNullOrWhiteSpace(filtre.Query)
                           || !string.IsNullOrWhiteSpace(filtre.Action)
                           || filtre.DateDebut.HasValue
                           || filtre.DateFin.HasValue;

                if (aFiltre)
                {
                    var filtreDto = _mapper.Map<AuditFiltreDto>(filtre);
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
                    Filtre = new AuditFiltreViewModel()
                });
            }
        }
    }
}