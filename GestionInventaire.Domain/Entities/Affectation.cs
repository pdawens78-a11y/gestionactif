using System.ComponentModel.DataAnnotations;
using GestionInventaire.Domain.Entities;

namespace GestionInventaire.Domain.Entities
{
    public class Affectation
    {
        [Key]
        public int IdAffectation { get; set; }

        // Relation - Actif affecté
        [Required(ErrorMessage = "L'actif est obligatoire")]
        public int IdActif { get; set; }
        public Actif Actif { get; set; } = null!;

        // Relation - Employé bénéficiaire
        [Required(ErrorMessage = "L'employé est obligatoire")]
        public int IdEmploye { get; set; }
        public Employe Employe { get; set; } = null!;

        // Dates
        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        // Propriété calculée - non stockée en base
        public bool EstActive => DateFin == null;
    }
}