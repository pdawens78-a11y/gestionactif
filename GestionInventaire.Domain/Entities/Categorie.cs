using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Domain.Entities
{
    public class Categorie
    {
        [Key]
        public int IdCategorie { get; set; }

        [Required]
        [StringLength(100)]
        public string NomCategorie { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}