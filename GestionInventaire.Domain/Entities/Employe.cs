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
        public string? Telephone { get; set; }

        // ── FK vers Service ──
        public int? IdService { get; set; }

        [ForeignKey(nameof(IdService))]
        public Service? Service { get; set; }

        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();

        // ── Propriétés calculées ──
        public string NomComplet => $"{Prenom} {Nom}".Trim();
        public string NomService => Service?.NomService ?? "—";
    }
}