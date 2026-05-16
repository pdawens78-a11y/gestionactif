using GestionInventaire.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionInventaire.DAL.Data
{
    public class AppDbContext : IdentityDbContext<Utilisateur>
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =========================
        // DBSETS
        // =========================

        public DbSet<Actif> Actifs { get; set; }

        public DbSet<Categorie> Categories { get; set; }

        public DbSet<Employe> Employes { get; set; }

        public DbSet<Produit> Produits { get; set; }

        public DbSet<Affectation> Affectations { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<MouvementStock> MouvementsStock { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        // =========================
        // MODEL CONFIGURATION
        // =========================

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // CATEGORIE → PRODUIT
            // =========================

            modelBuilder.Entity<Produit>()
                .HasOne(p => p.Categorie)
                .WithMany(c => c.Produits)
                .HasForeignKey(p => p.IdCategorie)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // PRODUIT → ACTIF
            // =========================

            modelBuilder.Entity<Actif>()
                .HasOne(a => a.Produit)
                .WithMany(p => p.Actifs)
                .HasForeignKey(a => a.IdProduit)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // AFFECTATION → ACTIF
            // =========================

            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Actif)
                .WithMany(a => a.Affectations)
                .HasForeignKey(a => a.IdActif)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // AFFECTATION → EMPLOYE
            // =========================

            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Employe)
                .WithMany(e => e.Affectations)
                .HasForeignKey(a => a.IdEmploye)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // STOCK → PRODUIT (1-1)
            // =========================

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Produit)
                .WithOne(p => p.Stock)
                .HasForeignKey<Stock>(s => s.IdProduit);

            // =========================
            // MOUVEMENT STOCK → STOCK
            // =========================

            modelBuilder.Entity<MouvementStock>()
                .HasOne(m => m.Stock)
                .WithMany(s => s.MouvementsStock)
                .HasForeignKey(m => m.IdStock)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // MAINTENANCE → ACTIF
            // =========================

            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Actif)
                .WithMany(a => a.Maintenances)
                .HasForeignKey(m => m.IdActif)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // DECIMAL PRECISION
            // =========================

            modelBuilder.Entity<Maintenance>()
                .Property(m => m.Cout)
                .HasPrecision(18, 2);

            // =========================
            // AUDIT LOG → UTILISATEUR
            // =========================

            modelBuilder.Entity<AuditLog>()
                .HasOne<Utilisateur>()
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.IdUtilisateur)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}