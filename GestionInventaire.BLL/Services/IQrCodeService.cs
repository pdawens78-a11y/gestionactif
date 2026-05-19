namespace GestionInventaire.BLL.Services
{
    public interface IQrCodeService
    {
        byte[] GenererQrCode(string texte);
    }
}