using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Domain.Entities
{
    public class Service
    {
        [Key]
        public int IdService { get; set; }

        [Required(ErrorMessage = "Le nom du service est obligatoire")]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string NomService { get; set; } = null!;

        [StringLength(300, ErrorMessage = "Maximum 300 caractères")]
        public string? Description { get; set; }
    }
}
