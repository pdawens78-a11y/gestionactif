using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Domain.Entities
{
    /// Représente une invitation pour créer un nouveau compte avec un rôle prédéfini.
    public class InvitationToken
    {
        [Key]
        public int IdInvitation { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(256)]
        public string Token { get; set; } = null!;

        /// <summary>
        /// Le rôle assigné à l'utilisateur (Technicien, Gestionnaire, Admin)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Role { get; set; } = null!;

        /// <summary>
        /// Date de création de l'invitation (défini automatiquement)
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Date d'expiration du token (30 jours après création)
        /// </summary>
        public DateTime DateExpiration { get; set; }

        /// <summary>
        /// Indique si l'invitation a été utilisée
        /// </summary>
        public bool Utilisee { get; set; } = false;

        /// <summary>
        /// Date d'utilisation de l'invitation (défini automatiquement quand l'utilisateur s'enregistre)
        /// </summary>
        public DateTime? DateUtilisation { get; set; }

        /// <summary>
        /// ID de l'utilisateur créé via cette invitation
        /// </summary>
        public string? IdUtilisateur { get; set; }

        // Navigation
        public Utilisateur? Utilisateur { get; set; }

        /// <summary>
        /// Vérifie si le token est encore valide
        /// </summary>
        public bool EstValide()
        {
            return !Utilisee && DateTime.Now <= DateExpiration;
        }
    }
}