using System.ComponentModel.DataAnnotations;
using GestionInventaire.Domain.Enums;

namespace GestionInventaire.Domain.Entities
{
    public class Maintenance
    {
        [Key]
        public int IdMaintenance { get; set; }

        [Required(ErrorMessage = "La date de maintenance est obligatoire")]
        public DateTime DateMaintenance { get; set; }

        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(500, ErrorMessage = "Maximum 500 caractères")]
        public string Description { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Le coût ne peut pas être négatif")]
        public decimal Cout { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire")]
        public StatutMaintenance Statut { get; set; }

        [Required(ErrorMessage = "L'actif est obligatoire")]
        public int IdActif { get; set; }
        public Actif Actif { get; set; } = null!;
    }
}