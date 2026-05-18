using GestionInventaire.BLL.Dtos;
using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;

namespace GestionInventaire.BLL.Services
{
    public class CategorieService : ICategorieService
    {
        private readonly ICategorieRepository _categorieRepository;
        private readonly IProduitRepository   _produitRepository;
        private readonly IAuditRepository     _auditRepository;

        public CategorieService(
            ICategorieRepository categorieRepository,
            IProduitRepository   produitRepository,
            IAuditRepository     auditRepository)
        {
            _categorieRepository = categorieRepository;
            _produitRepository   = produitRepository;
            _auditRepository     = auditRepository;
        }

        public async Task<CategorieListDto> GetAllCategoriesDtoAsync()
        {
            var categories = await _categorieRepository.GetAllAsync();
            var produits   = await _produitRepository.GetAllAsync();

            var produitsParCategorie = produits
                .GroupBy(p => p.IdCategorie)
                .ToDictionary(g => g.Key, g => g.Count());

            var items = categories
                .OrderBy(c => c.NomCategorie)
                .Select(c => new CategorieItemDto
                {
                    IdCategorie    = c.IdCategorie,
                    NomCategorie   = c.NomCategorie,
                    Description    = c.Description,
                    NombreProduits = produitsParCategorie.TryGetValue(c.IdCategorie, out var cnt)
                        ? cnt : 0
                })
                .ToList();

            return new CategorieListDto { Categories = items, TotalCount = items.Count };
        }

        public async Task<CategorieDetailDto> GetCategorieByIdAsync(int id)
        {
            var cat = await _categorieRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"La catégorie #{id} n'existe pas.");

            return new CategorieDetailDto
            {
                IdCategorie  = cat.IdCategorie,
                NomCategorie = cat.NomCategorie,
                Description  = cat.Description
            };
        }

        public async Task CreateCategorieAsync(CategorieCreateDto dto)
        {
            await ValidateNomAsync(dto.NomCategorie);

            var cat = new Categorie
            {
                NomCategorie = dto.NomCategorie.Trim(),
                Description  = dto.Description?.Trim()
            };

            await _categorieRepository.CreateAsync(cat);
            await _categorieRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Création catégorie : {cat.NomCategorie}", "Categorie", cat.IdCategorie);
        }

        public async Task UpdateCategorieAsync(CategorieEditDto dto)
        {
            var cat = await _categorieRepository.GetByIdAsync(dto.IdCategorie)
                ?? throw new InvalidOperationException($"La catégorie #{dto.IdCategorie} n'existe pas.");

            await ValidateNomAsync(dto.NomCategorie, dto.IdCategorie);

            cat.NomCategorie = dto.NomCategorie.Trim();
            cat.Description  = dto.Description?.Trim();

            _categorieRepository.Update(cat);
            await _categorieRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Modification catégorie : {cat.NomCategorie}", "Categorie", cat.IdCategorie);
        }

        public async Task DeleteCategorieAsync(int id)
        {
            var cat = await _categorieRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"La catégorie #{id} n'existe pas.");

            var produits = await _produitRepository.GetAllAsync();
            if (produits.Any(p => p.IdCategorie == id))
                throw new InvalidOperationException(
                    $"Impossible de supprimer « {cat.NomCategorie} » : des produits y sont rattachés.");

            _categorieRepository.Delete(cat);
            await _categorieRepository.SaveAsync();

            await _auditRepository.LogAsync(
                $"Suppression catégorie : {cat.NomCategorie}", "Categorie", id);
        }

        private async Task ValidateNomAsync(string nom, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom de la catégorie est obligatoire.");

            if (nom.Trim().Length < 2)
                throw new ArgumentException("Le nom doit contenir au moins 2 caractères.");

            var cats      = await _categorieRepository.GetAllAsync();
            var duplicate = cats.FirstOrDefault(c =>
                c.NomCategorie.Trim().ToLower() == nom.Trim().ToLower()
                && c.IdCategorie != (excludeId ?? 0));

            if (duplicate != null)
                throw new InvalidOperationException(
                    $"Une catégorie nommée « {nom.Trim()} » existe déjà.");
        }
    }
}
