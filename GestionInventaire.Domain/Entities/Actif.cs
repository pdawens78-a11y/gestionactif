using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestionInventaire.Domain.Enums;

namespace GestionInventaire.Domain.Entities
{
    public class Actif
    {
        [Key]
        public int IdActif { get; set; }

        [Required(ErrorMessage = "Le code inventaire est obligatoire")]
        [StringLength(50, ErrorMessage = "Maximum 50 caractères")]
        public string CodeInventaire { get; set; } = null!;

        [Required(ErrorMessage = "Le statut est obligatoire")]
        public StatutActif Statut { get; set; }

        [Required(ErrorMessage = "La localisation est obligatoire")]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string Localisation { get; set; } = null!;

        [Required(ErrorMessage = "La date d'acquisition est obligatoire")]
        public DateTime DateAcquisition { get; set; }

        [Required]
        [ForeignKey(nameof(Produit))]
        public int IdProduit { get; set; }
        public Produit Produit { get; set; } = null!;

        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}