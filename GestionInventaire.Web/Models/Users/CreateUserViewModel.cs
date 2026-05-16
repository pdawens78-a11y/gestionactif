using System.ComponentModel.DataAnnotations;

namespace GestionInventaire.Web.Models.Users
{
    public class CreateUserViewModel
    {
        [Required(
            ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(
            100,
            ErrorMessage =
                "Le nom ne peut pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        [Required(
            ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(
            100,
            ErrorMessage =
                "Le prénom ne peut pas dépasser 100 caractères.")]
        public string Prenom { get; set; }

        [Required(
            ErrorMessage =
                "L'adresse e-mail est obligatoire.")]
        [EmailAddress(
            ErrorMessage =
                "Adresse e-mail invalide.")]
        public string Email { get; set; }

        [Required(
            ErrorMessage =
                "Le téléphone est obligatoire.")]
        [Phone(
            ErrorMessage =
                "Numéro de téléphone invalide.")]
        public string Telephone { get; set; }

        [Required(
            ErrorMessage =
                "Veuillez sélectionner un rôle.")]
        public string Role { get; set; }
    }
}