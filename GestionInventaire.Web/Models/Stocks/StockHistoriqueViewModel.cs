namespace GestionInventaire.Web.Models.Stocks
{
    public class StockHistoriqueViewModel
    {
        public int IdStock { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;
        public int QuantiteActuelle { get; set; }
        public int SeuilAlerte { get; set; }
        public bool EstCritique => QuantiteActuelle <= SeuilAlerte;

        public List<MouvementRowViewModel> Mouvements { get; set; } = new();
    }

    public class MouvementRowViewModel
    {
        public int IdMouvement { get; set; }
        public DateTime DateMouvement { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Quantite { get; set; }

        public string DateFormatted =>
            DateMouvement.ToString("dd/MM/yyyy HH:mm");

        public string TypeLabel =>
            Type == "Entree" ? "Entrée" : "Sortie";

        public string TypeClass =>
            Type == "Entree" ? "mouvement-entree" : "mouvement-sortie";

        public string TypeIcon =>
            Type == "Entree" ? "bi-arrow-down-circle" : "bi-arrow-up-circle";
    }
}