using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository       _auditRepository;
        private readonly IUtilisateurRepository _utilisateurRepository;

        public AuditService(
            IAuditRepository       auditRepository,
            IUtilisateurRepository utilisateurRepository)
        {
            _auditRepository       = auditRepository;
            _utilisateurRepository = utilisateurRepository;
        }

        public async Task<AuditListDto> GetLogsAsync()
        {
            var logs         = await _auditRepository.GetLogsAsync();
            var utilisateurs = await _utilisateurRepository.GetAllAsync();
            var utilisateursParId = BuildDictionnaire(utilisateurs);
            return MapToDto(logs, utilisateursParId);
        }

        public async Task<AuditListDto> RechercherAsync(AuditFiltreDto filtre)
        {
            var logs = await _auditRepository.RechercherAsync(
                filtre.Query,
                filtre.Action,
                filtre.DateDebut,
                filtre.DateFin);

            var utilisateurs = await _utilisateurRepository.GetAllAsync();
            var utilisateursParId = BuildDictionnaire(utilisateurs);
            return MapToDto(logs, utilisateursParId);
        }

        // ── Helpers ──
        private static Dictionary<string, string> BuildDictionnaire(
            IEnumerable<Utilisateur> utilisateurs)
        {
            return utilisateurs.ToDictionary(
                u => u.Id,
                u =>
                {
                    var nom = u.NomComplet?.Trim();
                    if (!string.IsNullOrEmpty(nom)) return nom;
                    if (!string.IsNullOrEmpty(u.Email)) return u.Email;
                    if (!string.IsNullOrEmpty(u.UserName)) return u.UserName;
                    return $"Utilisateur #{u.Id[..8]}";
                });
        }

        private static AuditListDto MapToDto(
            List<AuditLog>             logs,
            Dictionary<string, string> utilisateursParId)
        {
            var items = logs.Select(l => new AuditLogItemDto
            {
                IdAuditLog     = l.IdAuditLog,
                Action         = l.Action,
                Entite         = l.Entite,
                EntiteId       = l.EntiteId,
                DateAction     = l.DateAction,
                IdUtilisateur  = l.IdUtilisateur,
                UtilisateurNom = utilisateursParId.TryGetValue(
                                     l.IdUtilisateur, out var nom)
                                 ? nom : "Système"
            }).ToList();

            return new AuditListDto
            {
                Logs       = items,
                TotalCount = items.Count
            };
        }
    }
}
