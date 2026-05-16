using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Web.Models.Stocks
{
    // ── Liste ──
    public class StockIndexViewModel
    {
        public List<StockRowViewModel> Stocks { get; set; } = new();
        public int TotalCount { get; set; }
        public int StocksCritiques { get; set; }
        public int SansStock { get; set; }
    }

    public class StockRowViewModel
    {
        public int IdStock { get; set; }
        public int IdProduit { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public int SeuilAlerte { get; set; }
        public bool EstCritique { get; set; }
        public int NombreActifs { get; set; }

        public string StatusClass =>
            Quantite == 0 ? "stock-zero"
            : EstCritique ? "stock-critical"
            : "stock-ok";

        public string StatusLabel =>
            Quantite == 0 ? "Épuisé"
            : EstCritique ? "Critique"
            : "Normal";

        public int PourcentageStock =>
            SeuilAlerte == 0 ? 100
            : Math.Min((int)((double)Quantite / (SeuilAlerte * 2) * 100), 100);
    }

    // ── Modification stock ──
    public class StockEditViewModel
    {
        public int IdStock { get; set; }
        public int IdProduit { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantité ne peut pas être négative")]
        [Display(Name = "Quantité en stock")]
        public int Quantite { get; set; }

        [Required(ErrorMessage = "Le seuil d'alerte est obligatoire")]
        [Range(0, int.MaxValue, ErrorMessage = "Le seuil ne peut pas être négatif")]
        [Display(Name = "Seuil d'alerte")]
        public int SeuilAlerte { get; set; }
    }

    // ── Mouvement de stock ──
    public class StockMouvementViewModel
    {
        public int IdStock { get; set; }
        public string NomProduit { get; set; } = string.Empty;
        public int QuantiteActuelle { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à zéro")]
        [Display(Name = "Quantité")]
        public int Quantite { get; set; }

        [Required(ErrorMessage = "Le type est obligatoire")]
        [Display(Name = "Type de mouvement")]
        public string Type { get; set; } = "Entree";
    }
}