using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace GestionInventaire.BLL.Services
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUtilisateurRepository  _utilisateurRepository;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuditRepository         _auditRepository;

        public UtilisateurService(
            IUtilisateurRepository   utilisateurRepository,
            UserManager<Utilisateur> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuditRepository         auditRepository)
        {
            _utilisateurRepository = utilisateurRepository;
            _userManager           = userManager;
            _roleManager           = roleManager;
            _auditRepository       = auditRepository;
        }

        public async Task<UtilisateurListDto> GetAllUtilisateursDtoAsync()
        {
            var utilisateurs = await _utilisateurRepository.GetAllAsync();

            var items = new List<UtilisateurItemDto>();

            foreach (var u in utilisateurs.OrderBy(u => u.Nom))
            {
                var roles = await _userManager.GetRolesAsync(u);

                items.Add(new UtilisateurItemDto
                {
                    Id            = u.Id,
                    Nom           = u.Nom,
                    Prenom        = u.Prenom,
                    NomComplet    = u.NomComplet,
                    Email         = u.Email ?? string.Empty,
                    Telephone     = u.PhoneNumber,
                    Role          = roles.FirstOrDefault() ?? "—",
                    EstVerrouille = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.Now,
                    EmailConfirme = u.EmailConfirmed
                });
            }

            return new UtilisateurListDto
            {
                Utilisateurs = items,
                TotalCount   = items.Count
            };
        }

        public async Task<UtilisateurEditDto> GetUtilisateurByIdAsync(string id)
        {
            var u = await _userManager.FindByIdAsync(id)
                ?? throw new InvalidOperationException($"L'utilisateur #{id} n'existe pas.");

            var roles = await _userManager.GetRolesAsync(u);

            return new UtilisateurEditDto
            {
                Id        = u.Id,
                Nom       = u.Nom,
                Prenom    = u.Prenom,
                Email     = u.Email ?? string.Empty,
                Telephone = u.PhoneNumber,
                Role      = roles.FirstOrDefault() ?? string.Empty
            };
        }

        public async Task<List<string>> GetRolesDisponiblesAsync()
        {
            return _roleManager.Roles
                .Select(r => r.Name!)
                .Where(n => !string.IsNullOrEmpty(n))
                .OrderBy(n => n)
                .ToList();
        }

        public async Task UpdateUtilisateurAsync(UtilisateurUpdateDto dto)
        {
            var u = await _userManager.FindByIdAsync(dto.Id)
                ?? throw new InvalidOperationException($"L'utilisateur #{dto.Id} n'existe pas.");

            u.Nom         = dto.Nom.Trim();
            u.Prenom      = dto.Prenom.Trim();
            u.PhoneNumber = dto.Telephone?.Trim();

            await _userManager.UpdateAsync(u);

            // Changer le rôle si nécessaire
            var rolesActuels = await _userManager.GetRolesAsync(u);
            var roleActuel   = rolesActuels.FirstOrDefault();

            if (roleActuel != dto.Role)
            {
                if (roleActuel != null)
                    await _userManager.RemoveFromRoleAsync(u, roleActuel);

                if (!string.IsNullOrEmpty(dto.Role))
                    await _userManager.AddToRoleAsync(u, dto.Role);
            }

            await _auditRepository.LogAsync(
                $"Modification utilisateur : {u.Email}", "Utilisateur", 0);
        }

        public async Task VerrouillerAsync(string id)
        {
            var u = await _userManager.FindByIdAsync(id)
                ?? throw new InvalidOperationException($"L'utilisateur #{id} n'existe pas.");

            // Verrouillage permanent — LockoutEnd dans 100 ans
            await _userManager.SetLockoutEndDateAsync(u, DateTimeOffset.Now.AddYears(100));
            await _userManager.SetLockoutEnabledAsync(u, true);

            await _auditRepository.LogAsync(
                $"Verrouillage utilisateur : {u.Email}", "Utilisateur", 0);
        }

        public async Task DeverrouillerAsync(string id)
        {
            var u = await _userManager.FindByIdAsync(id)
                ?? throw new InvalidOperationException($"L'utilisateur #{id} n'existe pas.");

            await _userManager.SetLockoutEndDateAsync(u, null);
            await _userManager.ResetAccessFailedCountAsync(u);

            await _auditRepository.LogAsync(
                $"Déverrouillage utilisateur : {u.Email}", "Utilisateur", 0);
        }

        public async Task SupprimerAsync(string id)
        {
            var u = await _userManager.FindByIdAsync(id)
                ?? throw new InvalidOperationException($"L'utilisateur #{id} n'existe pas.");

            // Bloquer la suppression du dernier Admin
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var roles  = await _userManager.GetRolesAsync(u);

            if (roles.Contains("Admin") && admins.Count <= 1)
                throw new InvalidOperationException(
                    "Impossible de supprimer le dernier administrateur du système.");

            await _userManager.DeleteAsync(u);

            await _auditRepository.LogAsync(
                $"Suppression utilisateur : {u.Email}", "Utilisateur", 0);
        }
    }
}
