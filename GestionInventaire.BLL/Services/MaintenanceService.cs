using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.Enums;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IActifRepository       _actifRepository;
        private readonly IProduitRepository     _produitRepository;
        private readonly IAuditRepository       _auditRepository;

        public MaintenanceService(
            IMaintenanceRepository maintenanceRepository,
            IActifRepository       actifRepository,
            IProduitRepository     produitRepository,
            IAuditRepository       auditRepository)
        {
            _maintenanceRepository = maintenanceRepository;
            _actifRepository       = actifRepository;
            _produitRepository     = produitRepository;
            _auditRepository       = auditRepository;
        }

        public async Task<MaintenanceListDto> GetAllMaintenancesDtoAsync()
        {
            var maintenances = await _maintenanceRepository.GetAllAsync();
            var actifs       = await _actifRepository.GetAllAsync();
            var produits     = await _produitRepository.GetAllAsync();

            var actifsParId   = actifs.ToDictionary(a => a.IdActif);
            var produitsParId = produits.ToDictionary(p => p.IdProduit, p => p.NomProduit);

            var items = maintenances
                .OrderByDescending(m => m.DateMaintenance)
                .Select(m =>
                {
                    actifsParId.TryGetValue(m.IdActif, out var actif);
                    var nomProduit = actif != null
                        && produitsParId.TryGetValue(actif.IdProduit, out var np)
                        ? np : "—";

                    return new MaintenanceItemDto
                    {
                        IdMaintenance   = m.IdMaintenance,
                        CodeActif       = actif?.CodeInventaire ?? $"Actif #{m.IdActif}",
                        NomProduit      = nomProduit,
                        DateMaintenance = m.DateMaintenance,
                        Description     = m.Description,
                        Cout            = m.Cout,
                        Statut          = m.Statut.ToString(),
                        EstUrgente      = m.Statut != StatutMaintenance.Terminee
                            && m.DateMaintenance <= DateTime.Now.AddDays(1)
                    };
                })
                .ToList();

            return new MaintenanceListDto
            {
                Maintenances = items,
                TotalCount   = items.Count,
                Planifiees   = items.Count(i => i.Statut == "Planifiee"),
                EnCours      = items.Count(i => i.Statut == "EnCours"),
                Terminees    = items.Count(i => i.Statut == "Terminee")
            };
        }

        public async Task<MaintenanceDetailDto> GetMaintenanceByIdAsync(int id)
        {
            var m = await _maintenanceRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"La maintenance #{id} n'existe pas.");

            var actif   = await _actifRepository.GetByIdAsync(m.IdActif);
            var produit = actif != null
                ? await _produitRepository.GetByIdAsync(actif.IdProduit)
                : null;

            return new MaintenanceDetailDto
            {
                IdMaintenance   = m.IdMaintenance,
                IdActif         = m.IdActif,
                CodeActif       = actif?.CodeInventaire ?? $"Actif #{m.IdActif}",
                NomProduit      = produit?.NomProduit ?? "—",
                DateMaintenance = m.DateMaintenance,
                Description     = m.Description,
                Cout            = m.Cout,
                Statut          = m.Statut.ToString()
            };
        }

        public async Task<List<ActifDisponibleMaintenanceDto>> GetActifsForMaintenanceAsync()
        {
            var actifs   = await _actifRepository.GetAllAsync();
            var produits = await _produitRepository.GetAllAsync();
            var produitsParId = produits.ToDictionary(p => p.IdProduit, p => p.NomProduit);

            return actifs
                .Where(a => a.Statut != StatutActif.HorsService)
                .OrderBy(a => a.CodeInventaire)
                .Select(a => new ActifDisponibleMaintenanceDto
                {
                    IdActif        = a.IdActif,
                    CodeInventaire = a.CodeInventaire,
                    NomProduit     = produitsParId.TryGetValue(a.IdProduit, out var n) ? n : "—",
                    Statut         = a.Statut.ToString()
                })
                .ToList();
        }

        public async Task CreateMaintenanceAsync(MaintenanceCreateDto dto)
        {
            var actif = await _actifRepository.GetByIdAsync(dto.IdActif)
                ?? throw new InvalidOperationException($"L'actif #{dto.IdActif} n'existe pas.");

            if (actif.Statut == StatutActif.HorsService)
                throw new InvalidOperationException("Un actif hors service ne peut pas être mis en maintenance.");

            var maintenance = new Maintenance
            {
                IdActif         = dto.IdActif,
                DateMaintenance = dto.DateMaintenance,
                Description     = dto.Description.Trim(),
                Cout            = dto.Cout,
                Statut          = StatutMaintenance.Planifiee
            };

            await _maintenanceRepository.CreateAsync(maintenance);

            actif.Statut = StatutActif.EnMaintenance;
            _actifRepository.Update(actif);

            await _maintenanceRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Maintenance planifiée : {actif.CodeInventaire}",
                "Maintenance", maintenance.IdMaintenance);
        }

        public async Task UpdateMaintenanceAsync(MaintenanceEditDto dto)
        {
            var m = await _maintenanceRepository.GetByIdAsync(dto.IdMaintenance)
                ?? throw new InvalidOperationException($"La maintenance #{dto.IdMaintenance} n'existe pas.");

            m.DateMaintenance = dto.DateMaintenance;
            m.Description     = dto.Description.Trim();
            m.Cout            = dto.Cout;

            _maintenanceRepository.Update(m);
            await _maintenanceRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Modification maintenance #{m.IdMaintenance}",
                "Maintenance", m.IdMaintenance);
        }

        public async Task DeleteMaintenanceAsync(int id)
        {
            var m = await _maintenanceRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"La maintenance #{id} n'existe pas.");

            // Libérer l'actif si la maintenance est supprimée
            var actif = await _actifRepository.GetByIdAsync(m.IdActif);
            if (actif != null && actif.Statut == StatutActif.EnMaintenance)
            {
                actif.Statut = StatutActif.Disponible;
                _actifRepository.Update(actif);
            }

            _maintenanceRepository.Delete(m);
            await _maintenanceRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Suppression maintenance #{id}", "Maintenance", id);
        }

        public async Task ChangerStatutAsync(int id, string nouveauStatut)
        {
            var m = await _maintenanceRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"La maintenance #{id} n'existe pas.");

            if (!Enum.TryParse<StatutMaintenance>(nouveauStatut, out var statut))
                throw new ArgumentException($"Statut invalide : {nouveauStatut}.");

            // Validation du cycle : Planifiee → EnCours → Terminee
            if (m.Statut == StatutMaintenance.Terminee)
                throw new InvalidOperationException("Une maintenance terminée ne peut pas changer de statut.");

            if (m.Statut == StatutMaintenance.EnCours && statut == StatutMaintenance.Planifiee)
                throw new InvalidOperationException("Impossible de revenir à l'état Planifiée.");

            m.Statut = statut;
            _maintenanceRepository.Update(m);

            // Si terminée → libérer l'actif
            if (statut == StatutMaintenance.Terminee)
            {
                var actif = await _actifRepository.GetByIdAsync(m.IdActif);
                if (actif != null)
                {
                    actif.Statut = StatutActif.Disponible;
                    _actifRepository.Update(actif);
                }
            }

            await _maintenanceRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Statut maintenance #{id} → {nouveauStatut}",
                "Maintenance", id);
        }
    }
}
