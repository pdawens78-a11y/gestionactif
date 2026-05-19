using GestionInventaire.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionInventaire.Web.Controllers
{
    [Authorize]
    public class QrCodeController : Controller
    {
        private readonly IQrCodeService _qrCodeService;

        public QrCodeController(IQrCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        // ════════════════════════════════════════════
        // GET /QrCode/Actif/{id}
        // ════════════════════════════════════════════
        public IActionResult Actif(int id)
        {
            var texte = $"ACTIF_{id}";
            var qrCodeBytes = _qrCodeService.GenererQrCode(texte);
            return File(qrCodeBytes, "image/png");
        }

        // ════════════════════════════════════════════
        // GET /QrCode/Produit/{id}
        // ════════════════════════════════════════════
        public IActionResult Produit(int id)
        {
            var texte = $"PRODUIT_{id}";
            var qrCodeBytes = _qrCodeService.GenererQrCode(texte);
            return File(qrCodeBytes, "image/png");
        }

        // ════════════════════════════════════════════
        // GET /QrCode/Stock/{id}
        // ════════════════════════════════════════════
        public IActionResult Stock(int id)
        {
            var texte = $"STOCK_{id}";
            var qrCodeBytes = _qrCodeService.GenererQrCode(texte);
            return File(qrCodeBytes, "image/png");
        }
    }
}