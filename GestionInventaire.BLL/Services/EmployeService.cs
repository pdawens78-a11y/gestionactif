using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository     _employeRepository;
        private readonly IAffectationRepository _affectationRepository;
        private readonly IServiceRepository     _serviceRepository;
        private readonly IAuditRepository       _auditRepository;

        public EmployeService(
            IEmployeRepository     employeRepository,
            IAffectationRepository affectationRepository,
            IServiceRepository     serviceRepository,
            IAuditRepository       auditRepository)
        {
            _employeRepository     = employeRepository;
            _affectationRepository = affectationRepository;
            _serviceRepository     = serviceRepository;
            _auditRepository       = auditRepository;
        }

        public async Task<EmployeListDto> GetAllEmployesDtoAsync()
        {
            var employes     = await _employeRepository.GetAllAsync();
            var affectations = await _affectationRepository.GetAllAsync();
            var services     = await _serviceRepository.GetAllAsync();

            var affParEmploye = affectations
                .GroupBy(a => a.IdEmploye)
                .ToDictionary(g => g.Key, g => g.ToList());

            var servicesParId = services.ToDictionary(s => s.IdService, s => s.NomService);

            var items = employes
                .OrderBy(e => e.Nom).ThenBy(e => e.Prenom)
                .Select(e =>
                {
                    affParEmploye.TryGetValue(e.IdEmploye, out var aff);
                    var liste      = aff ?? new();
                    var nomService = e.IdService.HasValue
                        && servicesParId.TryGetValue(e.IdService.Value, out var ns)
                        ? ns : "—";

                    return new EmployeItemDto
                    {
                        IdEmploye          = e.IdEmploye,
                        Nom                = e.Nom,
                        Prenom             = e.Prenom,
                        NomComplet         = $"{e.Prenom} {e.Nom}",
                        Email              = e.Email,
                        Telephone          = e.Telephone,
                        IdService          = e.IdService,
                        NomService         = nomService,
                        NombreAffectations = liste.Count,
                        ActifsActifs       = liste.Count(a => a.EstActive)
                    };
                })
                .ToList();

            return new EmployeListDto { Employes = items, TotalCount = items.Count };
        }

        public async Task<EmployeDetailDto> GetEmployeByIdAsync(int id)
        {
            var employe = await _employeRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"L'employé #{id} n'existe pas.");

            var affectations = await _affectationRepository.GetAllAsync();
            var aff          = affectations.Where(a => a.IdEmploye == id).ToList();
            var services     = await _serviceRepository.GetAllAsync();

            var nomService = employe.IdService.HasValue
                ? services.FirstOrDefault(s => s.IdService == employe.IdService.Value)?.NomService ?? "—"
                : "—";

            return new EmployeDetailDto
            {
                IdEmploye          = employe.IdEmploye,
                Nom                = employe.Nom,
                Prenom             = employe.Prenom,
                Email              = employe.Email,
                Telephone          = employe.Telephone,
                IdService          = employe.IdService,
                NomService         = nomService,
                NombreAffectations = aff.Count,
                ActifsActifs       = aff.Count(a => a.EstActive)
            };
        }

        public async Task CreateEmployeAsync(EmployeCreateDto dto)
        {
            ValidateDto(dto.Nom, dto.Prenom);

            var employe = new Employe
            {
                Nom       = dto.Nom.Trim(),
                Prenom    = dto.Prenom.Trim(),
                Email     = dto.Email?.Trim(),
                Telephone = dto.Telephone?.Trim(),
                IdService = dto.IdService
            };

            await _employeRepository.CreateAsync(employe);
            await _employeRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Création employé : {employe.Prenom} {employe.Nom}", "Employe", employe.IdEmploye);
        }

        public async Task UpdateEmployeAsync(EmployeEditDto dto)
        {
            var existing = await _employeRepository.GetByIdAsync(dto.IdEmploye)
                ?? throw new InvalidOperationException($"L'employé #{dto.IdEmploye} n'existe pas.");

            ValidateDto(dto.Nom, dto.Prenom);

            existing.Nom       = dto.Nom.Trim();
            existing.Prenom    = dto.Prenom.Trim();
            existing.Email     = dto.Email?.Trim();
            existing.Telephone = dto.Telephone?.Trim();
            existing.IdService = dto.IdService;

            _employeRepository.Update(existing);
            await _employeRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Modification employé : {existing.Prenom} {existing.Nom}", "Employe", existing.IdEmploye);
        }

        public async Task DeleteEmployeAsync(int id)
        {
            var employe = await _employeRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"L'employé #{id} n'existe pas.");

            var affectations = await _affectationRepository.GetAllAsync();
            var actives      = affectations.Count(a => a.IdEmploye == id && a.EstActive);

            if (actives > 0)
                throw new InvalidOperationException(
                    $"Impossible de supprimer {employe.Prenom} {employe.Nom} : " +
                    $"{actives} affectation(s) active(s) en cours.");

            _employeRepository.Delete(employe);
            await _employeRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Suppression employé : {employe.Prenom} {employe.Nom}", "Employe", id);
        }

        private static void ValidateDto(string nom, string prenom)
        {
            if (string.IsNullOrWhiteSpace(nom) || nom.Trim().Length < 2)
                throw new ArgumentException("Le nom est obligatoire (min. 2 caractères).");

            if (string.IsNullOrWhiteSpace(prenom) || prenom.Trim().Length < 2)
                throw new ArgumentException("Le prénom est obligatoire (min. 2 caractères).");
        }
    }
}
