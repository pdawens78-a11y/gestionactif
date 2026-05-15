using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionInventaire.Domain.Entities
{
    public class Stock
    {
        [Key]
        public int IdStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La quantité ne peut pas être négative")]
        public int Quantite { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le seuil d'alerte ne peut pas être négatif")]
        public int SeuilAlerte { get; set; }

        [Required(ErrorMessage = "Le produit est obligatoire")]
        [ForeignKey(nameof(Produit))]
        public int IdProduit { get; set; }
        public Produit Produit { get; set; } = null!;

        public ICollection<MouvementStock> MouvementsStock { get; set; } = new List<MouvementStock>();
    }
}