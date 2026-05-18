using GestionInventaire.Domain.IRepositories;
using GestionInventaire.BLL.Dtos;

// Alias pour éviter le conflit avec System.ServiceProcess
using DomainService = GestionInventaire.Domain.Entities.Service;

namespace GestionInventaire.BLL.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository  _serviceRepository;
        private readonly IEmployeRepository  _employeRepository;
        private readonly IAuditRepository    _auditRepository;

        public ServiceService(
            IServiceRepository  serviceRepository,
            IEmployeRepository  employeRepository,
            IAuditRepository    auditRepository)
        {
            _serviceRepository = serviceRepository;
            _employeRepository = employeRepository;
            _auditRepository   = auditRepository;
        }

        public async Task<ServiceListDto> GetAllServicesDtoAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            var employes = await _employeRepository.GetAllAsync();

            var employesParService = employes
                .Where(e => e.IdService.HasValue)
                .GroupBy(e => e.IdService!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var items = services
                .OrderBy(s => s.NomService)
                .Select(s => new ServiceItemDto
                {
                    IdService      = s.IdService,
                    NomService     = s.NomService,
                    Description    = s.Description,
                    NombreEmployes = employesParService.TryGetValue(s.IdService, out var cnt)
                        ? cnt : 0
                })
                .ToList();

            return new ServiceListDto { Services = items, TotalCount = items.Count };
        }

        public async Task<ServiceDetailDto> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Le service #{id} n'existe pas.");

            return new ServiceDetailDto
            {
                IdService   = service.IdService,
                NomService  = service.NomService,
                Description = service.Description
            };
        }

        public async Task<List<ServiceSelectDto>> GetServicesSelectAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services
                .OrderBy(s => s.NomService)
                .Select(s => new ServiceSelectDto
                {
                    IdService  = s.IdService,
                    NomService = s.NomService
                })
                .ToList();
        }

        public async Task CreateServiceAsync(ServiceCreateDto dto)
        {
            await ValidateNomAsync(dto.NomService);

            var service = new DomainService
            {
                NomService  = dto.NomService.Trim(),
                Description = dto.Description?.Trim()
            };

            await _serviceRepository.CreateAsync(service);
            await _serviceRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Création service : {service.NomService}", "Service", service.IdService);
        }

        public async Task UpdateServiceAsync(ServiceEditDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.IdService)
                ?? throw new InvalidOperationException($"Le service #{dto.IdService} n'existe pas.");

            await ValidateNomAsync(dto.NomService, dto.IdService);

            service.NomService  = dto.NomService.Trim();
            service.Description = dto.Description?.Trim();

            _serviceRepository.Update(service);
            await _serviceRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Modification service : {service.NomService}", "Service", service.IdService);
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Le service #{id} n'existe pas.");

            var employes = await _employeRepository.GetAllAsync();
            var rattaches = employes.Where(e => e.IdService == id).ToList();

            if (rattaches.Any())
                throw new InvalidOperationException(
                    $"Impossible de supprimer « {service.NomService} » : " +
                    $"{rattaches.Count} employé(s) y sont rattachés.");

            _serviceRepository.Delete(service);
            await _serviceRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Suppression service : {service.NomService}", "Service", id);
        }

        private async Task ValidateNomAsync(string nom, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(nom) || nom.Trim().Length < 2)
                throw new ArgumentException("Le nom du service est obligatoire (min. 2 caractères).");

            var services  = await _serviceRepository.GetAllAsync();
            var duplicate = services.FirstOrDefault(s =>
                s.NomService.Trim().ToLower() == nom.Trim().ToLower()
                && s.IdService != (excludeId ?? 0));

            if (duplicate != null)
                throw new InvalidOperationException($"Un service nommé « {nom.Trim()} » existe déjà.");
        }
    }
}
