using System.Security.Principal;

namespace GestionInventaire.Web.Helpers
{
    /// <summary>
    /// Helper pour vérifier les permissions des utilisateurs selon leurs rôles
    /// </summary>
    public static class AuthorizationHelper
    {
        /// <summary>
        /// Vérifier si l'utilisateur peut gérer les actifs (Admin, Gestionnaire, Technicien)
        /// </summary>
        public static bool CanManageActifs(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire") || user.IsInRole("Technicien");

        /// <summary>
        /// Vérifier si l'utilisateur peut gérer les produits (Admin, Gestionnaire)
        /// </summary>
        public static bool CanManageProduits(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire");

        /// <summary>
        /// Vérifier si l'utilisateur peut gérer le stock (Admin, Gestionnaire)
        /// </summary>
        public static bool CanManageStock(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire");

        /// <summary>
        /// Vérifier si l'utilisateur peut gérer les employés (Admin, Gestionnaire)
        /// </summary>
        public static bool CanManageEmployes(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire");

        /// <summary>
        /// Vérifier si l'utilisateur peut gérer les affectations (Admin, Gestionnaire, Technicien)
        /// </summary>
        public static bool CanManageAffectations(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire") || user.IsInRole("Technicien");

        /// <summary>
        /// Vérifier si l'utilisateur peut gérer les maintenances (Admin, Gestionnaire, Technicien)
        /// </summary>
        public static bool CanManageMaintenance(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire") || user.IsInRole("Technicien");

        /// <summary>
        /// Vérifier si l'utilisateur peut voir les rapports (Admin, Gestionnaire)
        /// </summary>
        public static bool CanViewRapports(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire");

        /// <summary>
        /// Vérifier si l'utilisateur peut voir l'audit (Admin uniquement)
        /// </summary>
        public static bool CanViewAudit(this IPrincipal user)
            => user.IsInRole("Admin");

        /// <summary>
        /// Vérifier si l'utilisateur peut gérer les utilisateurs (Admin uniquement)
        /// </summary>
        public static bool CanManageUsers(this IPrincipal user)
            => user.IsInRole("Admin");

        /// <summary>
        /// Vérifier si l'utilisateur peut accéder aux paramètres (Admin, Gestionnaire)
        /// </summary>
        public static bool CanAccessSettings(this IPrincipal user)
            => user.IsInRole("Admin") || user.IsInRole("Gestionnaire");
    }
}
