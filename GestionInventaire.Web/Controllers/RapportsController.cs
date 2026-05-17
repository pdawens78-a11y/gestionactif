using AutoMapper;
using GestionInventaire.BLL.Services;
using GestionInventaire.Web.Models.Rapports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GestionInventaire.Web.Controllers
{
    [Authorize(Roles = "Admin,Gestionnaire")]
    public class RapportsController : Controller
    {
        private readonly IRapportService _rapportService;
        private readonly IMapper _mapper;
        private readonly ILogger<RapportsController> _logger;

        public RapportsController(
            IRapportService rapportService,
            IMapper mapper,
            ILogger<RapportsController> logger)
        {
            _rapportService = rapportService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET /Rapports
        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _rapportService.GetRapportAsync();
                var vm = _mapper.Map<RapportIndexViewModel>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur chargement rapport");
                TempData["Erreur"] = "Erreur lors de la génération du rapport.";
                return View(new RapportIndexViewModel());
            }
        }

        // GET /Rapports/ExportCsv?section=inventaire
        public async Task<IActionResult> ExportCsv(string section = "inventaire")
        {
            try
            {
                var dto = await _rapportService.GetRapportAsync();
                var sb = new StringBuilder();
                var nom = "";

                switch (section.ToLower())
                {
                    case "inventaire":
                        nom = "inventaire_actifs";
                        sb.AppendLine("Code Inventaire;Produit;Catégorie;Localisation;Statut;Date Acquisition");
                        foreach (var a in dto.Inventaire.Actifs)
                            sb.AppendLine(
                                $"{a.CodeInventaire};" +
                                $"{Escape(a.NomProduit)};" +
                                $"{Escape(a.NomCategorie)};" +
                                $"{Escape(a.Localisation)};" +
                                $"{a.Statut};" +
                                $"{a.DateAcquisition:dd/MM/yyyy}");
                        break;

                    case "stock":
                        nom = "etat_stock";
                        sb.AppendLine("Produit;Catégorie;Quantité;Seuil Alerte;Statut");
                        foreach (var s in dto.Stock.Stocks)
                            sb.AppendLine(
                                $"{Escape(s.NomProduit)};" +
                                $"{Escape(s.NomCategorie)};" +
                                $"{s.Quantite};" +
                                $"{s.SeuilAlerte};" +
                                $"{(s.EstEpuise ? "Épuisé" : s.EstCritique ? "Critique" : "Normal")}");
                        break;

                    case "affectations":
                        nom = "affectations_actives";
                        sb.AppendLine("Code Actif;Produit;Employé;Service;Date Début;Durée (jours)");
                        foreach (var a in dto.Affectations.Affectations)
                            sb.AppendLine(
                                $"{a.CodeActif};" +
                                $"{Escape(a.NomProduit)};" +
                                $"{Escape(a.NomEmploye)};" +
                                $"{Escape(a.Service)};" +
                                $"{a.DateDebut:dd/MM/yyyy};" +
                                $"{a.DureeJours}");
                        break;

                    case "maintenances":
                        nom = "rapport_maintenances";
                        sb.AppendLine("Code Actif;Produit;Date;Statut;Coût (HTG);Description");
                        foreach (var m in dto.Maintenances.Maintenances)
                            sb.AppendLine(
                                $"{m.CodeActif};" +
                                $"{Escape(m.NomProduit)};" +
                                $"{m.DateMaintenance:dd/MM/yyyy};" +
                                $"{m.Statut};" +
                                $"{m.Cout:N2};" +
                                $"{Escape(m.Description)}");
                        break;

                    default:
                        return BadRequest("Section inconnue.");
                }

                var bytes = Encoding.UTF8.GetPreamble()
                    .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                    .ToArray();
                var fileName = $"{nom}_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur export CSV section={Section}", section);
                TempData["Erreur"] = "Erreur lors de l'export CSV.";
                return RedirectToAction(nameof(Index));
            }
        }

        private static string Escape(string val) =>
            val.Contains(';') || val.Contains('"')
                ? $"\"{val.Replace("\"", "\"\"")}\""
                : val;
    }
}