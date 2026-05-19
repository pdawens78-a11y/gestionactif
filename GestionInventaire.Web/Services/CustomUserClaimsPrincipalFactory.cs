using GestionInventaire.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GestionInventaire.Web.Services
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<Utilisateur, IdentityRole>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<Utilisateur> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(Utilisateur user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            // ── Ajouter le prénom (GivenName) ──
            if (!string.IsNullOrEmpty(user.Prenom))
            {
                identity?.AddClaim(new Claim(ClaimTypes.GivenName, user.Prenom));
            }

            // ── Ajouter le nom (Surname) ──
            if (!string.IsNullOrEmpty(user.Nom))
            {
                identity?.AddClaim(new Claim(ClaimTypes.Surname, user.Nom));
            }

            return principal;
        }
    }
}