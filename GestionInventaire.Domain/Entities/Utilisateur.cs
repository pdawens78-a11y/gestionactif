
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Domain.Entities
{
    public class Utilisateur : IdentityUser
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string Nom { get; set; } = null!;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, ErrorMessage = "Maximum 100 caractères")]
        public string Prenom { get; set; } = null!;

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        [StringLength(20)]
        public string? Telephone { get; set; }

        // Propriété calculée - non stockée en base
        public string NomComplet => $"{Prenom} {Nom}";

        // Navigation 
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
       
    }
}