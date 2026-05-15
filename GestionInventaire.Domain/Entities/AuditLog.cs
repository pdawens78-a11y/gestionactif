using System.ComponentModel.DataAnnotations;
namespace GestionInventaire.Domain.Entities
{
    public class AuditLog
    {
        [Key]
        public int IdAuditLog { get; set; }

        [Required(ErrorMessage = "L'action est obligatoire")]
        [StringLength(200, ErrorMessage = "Maximum 200 caractères")]
        public string Action { get; set; } = null!;

        public DateTime DateAction { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "L'entité est obligatoire")]
        [StringLength(100)]
        public string Entite { get; set; } = null!;

        public int EntiteId { get; set; }

        [Required]
        public string IdUtilisateur { get; set; } = null!;
    }
}