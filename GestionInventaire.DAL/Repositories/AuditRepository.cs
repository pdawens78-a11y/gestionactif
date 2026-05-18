using GestionInventaire.DAL.Data;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GestionInventaire.DAL.Repositories
{
    public class AuditRepository : GenericRepository<AuditLog>, IAuditRepository
    {
        public AuditRepository(AppDbContext context) : base(context)
        {
        }

        public List<AuditLog> GetLogs()
        {
            return _context.AuditLogs
                .AsNoTracking()
                .OrderByDescending(a => a.DateAction)
                .ToList();
        }

        public Task<List<AuditLog>> GetLogsAsync()
        {
            return Task.FromResult(GetLogs());
        }

        public void Log(string action, string entite, int entiteId)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("L'action est obligatoire", nameof(action));

            if (string.IsNullOrWhiteSpace(entite))
                throw new ArgumentException("L'entité est obligatoire", nameof(entite));

            // Votre modèle AuditLog exige IdUtilisateur + FK vers AspNetUsers.
            // Comme l'interface ne reçoit pas l'utilisateur, on prend un utilisateur existant.
            var utilisateurId = _context.Users
                .Select(u => u.Id)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(utilisateurId))
                throw new InvalidOperationException(
                    "Aucun utilisateur trouvé pour journaliser l'audit. " +
                    "Créez un utilisateur ou adaptez IAuditRepository.Log pour accepter IdUtilisateur.");

            var log = new AuditLog
            {
                Action = action.Trim(),
                Entite = entite.Trim(),
                EntiteId = entiteId,
                DateAction = DateTime.Now,
                IdUtilisateur = utilisateurId
            };

            _context.AuditLogs.Add(log);
            _context.SaveChanges();
        }

        public Task LogAsync(string action, string entite, int entiteId)
        {
            Log(action, entite, entiteId);
            return Task.CompletedTask;
        }

        public List<AuditLog> Rechercher(string? query, string? action, DateTime? dateDebut, DateTime? dateFin)
        {
            var logs = _context.AuditLogs
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var q = query.Trim().ToLower();
                logs = logs.Where(a =>
                    a.Action.ToLower().Contains(q) ||
                    a.Entite.ToLower().Contains(q));
            }

            if (!string.IsNullOrWhiteSpace(action))
            {
                var a = action.Trim().ToLower();
                logs = logs.Where(l => l.Action.ToLower().Contains(a));
            }

            if (dateDebut.HasValue)
            {
                var debut = dateDebut.Value.Date;
                logs = logs.Where(l => l.DateAction >= debut);
            }

            if (dateFin.HasValue)
            {
                var finInclus = dateFin.Value.Date.AddDays(1).AddTicks(-1);
                logs = logs.Where(l => l.DateAction <= finInclus);
            }

            return logs
                .OrderByDescending(l => l.DateAction)
                .ToList();
        }

        public Task<List<AuditLog>> RechercherAsync(string? query, string? action, DateTime? dateDebut, DateTime? dateFin)
        {
            return Task.FromResult(Rechercher(query, action, dateDebut, dateFin));
        }
    }
}