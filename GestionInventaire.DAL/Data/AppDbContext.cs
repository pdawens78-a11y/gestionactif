using GestionInventaire.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionInventaire.DAL.Data
{
    public class AppDbContext : IdentityDbContext<Utilisateur>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Actif> Actifs { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Affectation> Affectations { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MouvementStock> MouvementsStock { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<InvitationToken> InvitationTokens { get; set; }

        // Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // InvitationToken → Utilisateur
            modelBuilder.Entity<InvitationToken>()
                .HasOne(i => i.Utilisateur)
                .WithOne(u => u.InvitationToken)
                .HasForeignKey<InvitationToken>(i => i.IdUtilisateur)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indices
            modelBuilder.Entity<InvitationToken>()
                .HasIndex(i => i.Email);

            modelBuilder.Entity<InvitationToken>()
                .HasIndex(i => i.Token)
                .IsUnique();

            // Actif → Categorie
            modelBuilder.Entity<Actif>()
                .HasOne(a => a.Categorie)
                .WithMany(c => c.Actifs)
                .HasForeignKey(a => a.IdCategorie)
                .OnDelete(DeleteBehavior.Restrict);

            // Affectation → Actif
            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Actif)
                .WithMany(a => a.Affectations)
                .HasForeignKey(a => a.IdActif)
                .OnDelete(DeleteBehavior.Restrict);

            // Affectation → Employe
            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Employe)
                .WithMany(e => e.Affectations)
                .HasForeignKey(a => a.IdEmploye)
                .OnDelete(DeleteBehavior.Restrict);

            // Stock → Actif (1-1)
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Actif)
                .WithOne(a => a.Stock)
                .HasForeignKey<Stock>(s => s.IdActif);

            // MouvementStock → Stock
            modelBuilder.Entity<MouvementStock>()
                .HasOne(m => m.Stock)
                .WithMany(s => s.MouvementsStock)
                .HasForeignKey(m => m.IdStock)
                .OnDelete(DeleteBehavior.Cascade);

            // Maintenance → Actif
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Actif)
                .WithMany(a => a.Maintenances)
                .HasForeignKey(m => m.IdActif)
                .OnDelete(DeleteBehavior.Restrict);

            // FIX DECIMAL
            modelBuilder.Entity<Maintenance>()
                .Property(m => m.Cout)
                .HasPrecision(18, 2);

            // AuditLog → Utilisateur
            modelBuilder.Entity<AuditLog>()
                .HasOne<Utilisateur>()
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.IdUtilisateur)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}