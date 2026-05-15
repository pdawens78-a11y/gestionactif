using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Domain.Entities
{
    public class Employe
    {
        [Key]
        public int IdEmploye { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string Nom { get; set; } = null!;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string Prenom { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        [StringLength(20)]
        public string? Tel { get; set; }

        [StringLength(100)]
        public string? Service { get; set; }

        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }
}