using System.ComponentModel.DataAnnotations;
using GestionInventaire.Domain.Enums;

namespace GestionInventaire.Domain.Entities
{
    public class MouvementStock
    {
        [Key]
        public int IdMouvement { get; set; }

        public DateTime DateMouvement { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le type de mouvement est obligatoire")]
        public TypeMouvement Type { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à zéro")]
        public int Quantite { get; set; }

        [Required(ErrorMessage = "Le stock est obligatoire")]
        public int IdStock { get; set; }
        public Stock Stock { get; set; } = null!;
    }
}