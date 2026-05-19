using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionInventaire.BLL.Services
{
    public class QrCodeService : IQrCodeService
    {
        public byte[] GenererQrCode(string texte)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(texte, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrData);
            return qrCode.GetGraphic(20);
        }
    }
}
