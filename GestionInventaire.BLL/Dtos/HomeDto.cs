namespace GestionInventaire.BLL.Dtos
{
    public class HomeDto
    {
        public int TotalActifs { get; set; }
        public int ActifsDisponibles { get; set; }
        public int ActifsAffectes { get; set; }
        public int ActifsEnMaintenance { get; set; }
        public int TotalProduits { get; set; }
        public int TotalEmployes { get; set; }
        public int StocksCritiques { get; set; }
    }
}