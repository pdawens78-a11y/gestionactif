using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string? Telephone
        {
            get => Tel;
            set => Tel = value;
        }

        [StringLength(100)]
        public string? Service { get; set; }

        [NotMapped]
        public int? IdService
        {
            get => int.TryParse(Service, out var id) ? id : null;
            set => Service = value?.ToString();
        }

        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }
}