using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionInventaire.Domain.Entities
{
    public class Produit
    {
        [Key]
        public int IdProduit { get; set; }

        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string NomProduit { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [ForeignKey(nameof(Categorie))]
        public int IdCategorie { get; set; }
        public Categorie Categorie { get; set; } = null!;

        // Relations
        public Stock? Stock { get; set; }
        public ICollection<Actif> Actifs { get; set; } = new List<Actif>();
    }
}