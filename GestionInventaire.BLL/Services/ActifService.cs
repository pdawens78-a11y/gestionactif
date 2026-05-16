using GestionInventaire.Domain.Entities;
using GestionInventaire.Domain.IRepositories;
using GestionInventaire.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionInventaire.BLL.Services
{
    public class ActifService : IActifService
    {
        private readonly IActifRepository _actifRepository;
        private readonly IProduitRepository _produitRepository;
        private readonly IAuditRepository _auditRepository;

        public ActifService(
            IActifRepository actifRepository,
            IProduitRepository produitRepository,
            IAuditRepository auditRepository)
        {
            _actifRepository = actifRepository;
            _produitRepository = produitRepository;
            _auditRepository = auditRepository;
        }

        private async Task ValidateActifAsync(Actif actif)
        {
            if (actif == null)
                throw new ArgumentNullException(nameof(actif));

            if (actif.IdProduit <= 0)
                throw new ArgumentException("Le produit est invalide");

            // Vérifier que le produit existe
            var produit = await _produitRepository.GetByIdAsync(actif.IdProduit);
            if (produit == null)
                throw new InvalidOperationException($"Le produit avec l'ID {actif.IdProduit} n'existe pas");

            if (string.IsNullOrWhiteSpace(produit.NomProduit))
                throw new ArgumentException("Le nom du produit est obligatoire");

            if (string.IsNullOrWhiteSpace(actif.Localisation))
                throw new ArgumentException("La localisation est obligatoire");

            if (actif.Localisation.Length < 3)
                throw new ArgumentException("La localisation doit contenir au moins 3 caractères");
        }

        public async Task<IEnumerable<Actif>> GetAllActifsAsync()
        {
            return await _actifRepository.GetAllAsync();
        }

        public async Task<Actif> GetActifByIdAsync(int id)
        {
            var actif = await _actifRepository.GetByIdAsync(id);
            if (actif == null)
                throw new InvalidOperationException($"L'actif avec l'ID {id} n'existe pas");

            return actif;
        }

        public async Task<Actif> CreateActifAsync(Actif actif)
        {
            await ValidateActifAsync(actif);

            // Récupérer le produit pour utiliser son nom
            var produit = await _produitRepository.GetByIdAsync(actif.IdProduit);
            if (produit == null)
                throw new InvalidOperationException($"Le produit avec l'ID {actif.IdProduit} n'existe pas");

            // Générer automatiquement le code inventaire
            actif.CodeInventaire = await GenerateCodeInventaireAsync(actif.IdProduit, produit.NomProduit);

            // Vérifier que le code inventaire généré est unique (sécurité supplémentaire)
            var allActifs = await _actifRepository.GetAllAsync();
            var existing = allActifs.Where(a => a.CodeInventaire == actif.CodeInventaire);
            if (existing.Any())
                throw new InvalidOperationException($"Un actif avec le code {actif.CodeInventaire} existe déjà");

            // Définir le statut par défaut
            actif.Statut = StatutActif.Disponible;

            await _actifRepository.CreateAsync(actif);
            await _actifRepository.SaveAsync();

            // Enregistrer l'audit
            _auditRepository.Log("Création Actif", "Actif", actif.IdActif);

            return actif;
        }

        public async Task<Actif> UpdateActifAsync(Actif actif)
        {
            if (actif == null)
                throw new ArgumentNullException(nameof(actif));

            var existing = await _actifRepository.GetByIdAsync(actif.IdActif);
            if (existing == null)
                throw new InvalidOperationException($"L'actif avec l'ID {actif.IdActif} n'existe pas");

            // Valider les propriétés modifiables
            if (string.IsNullOrWhiteSpace(actif.Localisation))
                throw new ArgumentException("La localisation est obligatoire");

            // Le code inventaire et l'IdProduit ne peuvent pas être modifiés
            actif.CodeInventaire = existing.CodeInventaire;
            actif.IdProduit = existing.IdProduit;

            _actifRepository.Update(actif);
            await _actifRepository.SaveAsync();

            // Enregistrer l'audit
            _auditRepository.Log("Modification Actif", "Actif", actif.IdActif);

            return actif;
        }

        public async Task DeleteActifAsync(int id)
        {
            var actif = await _actifRepository.GetByIdAsync(id);
            if (actif == null)
                throw new InvalidOperationException($"L'actif avec l'ID {id} n'existe pas");

            // Vérifier que l'actif peut être supprimé
            if (actif.Statut == StatutActif.Affecte)
                throw new InvalidOperationException("Impossible de supprimer un actif affecté");

            if (actif.Statut == StatutActif.EnMaintenance)
                throw new InvalidOperationException("Impossible de supprimer un actif en maintenance");

            _actifRepository.Delete(actif);
            await _actifRepository.SaveAsync();

            // Enregistrer l'audit
            _auditRepository.Log("Suppression Actif", "Actif", id);
        }

        public async Task<IEnumerable<Actif>> GetActifsByProductAsync(int productId)
        {
            // Vérifier que le produit existe
            var produit = await _produitRepository.GetByIdAsync(productId);
            if (produit == null)
                throw new InvalidOperationException($"Le produit avec l'ID {productId} n'existe pas");

            var allActifs = await _actifRepository.GetAllAsync();
            return allActifs.Where(a => a.IdProduit == productId).ToList();
        }

        public async Task<IEnumerable<Actif>> GetActifsByLocalisationAsync(string localisation)
        {
            if (string.IsNullOrWhiteSpace(localisation))
                throw new ArgumentException("La localisation ne peut pas être vide");

            var allActifs = await _actifRepository.GetAllAsync();
            return allActifs.Where(a => a.Localisation.Contains(localisation)).ToList();
        }

        public async Task<IEnumerable<Actif>> GetActifsByStatusAsync(StatutActif status)
        {
            var allActifs = await _actifRepository.GetAllAsync();
            return allActifs.Where(a => a.Statut == status).ToList();
        }

        /// <summary>
        /// Génère automatiquement le code inventaire au format : [Première lettre du nom du produit en majuscule][3 chiffres]
        /// Exemple : P001, P002, A001, M003, etc.
        /// </summary>
        private async Task<string> GenerateCodeInventaireAsync(int productId, string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Le nom du produit ne peut pas être vide");

            // Récupérer la première lettre du nom du produit en majuscule
            char firstLetter = char.ToUpper(productName[0]);

            // Récupérer tous les actifs du produit pour trouver le numéro suivant
            var allActifs = await _actifRepository.GetAllAsync();
            var actifsDuProduit = allActifs.Where(a => a.IdProduit == productId).ToList();

            // Extraire les numéros existants des codes inventaires commençant par la même lettre
            var existingNumbers = actifsDuProduit
                .Where(a => !string.IsNullOrWhiteSpace(a.CodeInventaire)
                    && a.CodeInventaire.Length >= 4
                    && char.ToUpper(a.CodeInventaire[0]) == firstLetter)
                .Select(a =>
                {
                    // Essayer de parser les 3 chiffres après la lettre
                    if (int.TryParse(a.CodeInventaire.Substring(1, 3), out int number))
                        return number;
                    return 0;
                })
                .Where(n => n > 0)
                .ToList();

            // Déterminer le prochain numéro
            int nextNumber = existingNumbers.Count > 0 ? existingNumbers.Max() + 1 : 1;

            // Générer le code au format : Lettre + 3 chiffres (ex: P001, P002, A001)
            return $"{firstLetter}{nextNumber:D3}";
        }
    }
}