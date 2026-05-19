# TechnoLogis S.A. — Système de Gestion Intelligente d'Inventaire et d'Actifs

> Application web ASP.NET Core MVC développée dans le cadre du cours **Framework .NET** (PDR 6)  
> **Titulaire :** Gregory SOLIDE · **Étudiant :** Dawens H. PIERRE · **MBDS-Haiti 2025-2026**  
> **Client :** Madame Valdaya PRINCIVIL, Directrice Générale de TechnoLogis S.A.

---

## Table des matières

- [Aperçu](#aperçu)
- [Technologies utilisées](#technologies-utilisées)
- [Architecture](#architecture)
- [Prérequis](#prérequis)
- [Installation](#installation)
- [Configuration](#configuration)
- [Comptes par défaut](#comptes-par-défaut)
- [Modules fonctionnels](#modules-fonctionnels)
- [Modèle de données](#modèle-de-données)
- [Règles métier importantes](#règles-métier-importantes)
- [Sécurité](#sécurité)

---

## Aperçu

TechnoLogis est un système centralisé de gestion d'inventaire et d'actifs informatiques permettant de :

- Gérer le **catalogue produits** avec génération automatique des actifs et du stock
- Suivre le **cycle de vie complet** des actifs (acquisition → affectation → maintenance → réforme)
- Gérer les **affectations** par employé et par département
- Planifier et suivre les **maintenances** avec gestion des coûts
- Générer des **rapports opérationnels** exportables en CSV
- Tracer toutes les actions via un **journal d'audit** complet
- Gérer les **utilisateurs** via ASP.NET Identity avec invitation par email

---

## Technologies utilisées

| Technologie | Version | Usage |
|---|---|---|
| ASP.NET Core MVC | .NET 9 | Framework web |
| Entity Framework Core | 9.x | ORM — Code First |
| Microsoft SQL Server | 2019+ | Base de données |
| ASP.NET Core Identity | 9.x | Authentification & rôles |
| AutoMapper | 16.x | Mapping DTO ↔ ViewModel |
| Mailjet | API v3 | Envoi d'emails (invitation) |
| Bootstrap Icons | 1.11 | Icônes UI |
| Chart.js | 4.4 | Graphiques Dashboard |

---

## Architecture

L'application est organisée en **4 couches** strictement séparées :

```
GestionInventaire.Domain      ← Entités, IRepositories, Enums
GestionInventaire.DAL         ← AppDbContext, Repositories, Migrations
GestionInventaire.BLL         ← Services métier, DTOs, Validations
GestionInventaire.Web         ← Controllers, ViewModels, Vues Razor, AutoMapper
```

**Règles d'architecture :**
- Les controllers ne contiennent **aucune logique métier**
- Les services BLL ne contiennent **aucune logique de présentation**
- Toutes les requêtes EF Core sont **séquentielles** (pas de `Task.WhenAll` sur le même DbContext)
- Les transactions sont **atomiques** (minimum de `SaveAsync`)

---

## Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server 2019+](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads) ou SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommandé) ou VS Code
- Compte [Mailjet](https://www.mailjet.com/) pour l'envoi d'emails (optionnel en dev)

---

## Installation

### 1. Cloner le dépôt

```bash
git clone https://github.com/votre-repo/GestionInventaire.git
cd GestionInventaire
```

### 2. Configurer la base de données

Dans `GestionInventaire.Web/appsettings.json`, mettez à jour la chaîne de connexion :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=GestionInventaire;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Configurer Mailjet (optionnel)

```json
"Mailjet": {
  "ApiKey": "votre-api-key",
  "ApiSecret": "votre-api-secret",
  "SenderEmail": "noreply@technologis.com",
  "SenderName": "TechnoLogis"
}
```

> Si Mailjet n'est pas configuré, remplacez `MailjetEmailSender` par `NoOpEmailSender` dans `Program.cs`.

### 4. Appliquer les migrations

Dans le **Package Manager Console** (projet DAL sélectionné) :

```
Add-Migration InitialCreate
Update-Database
```

### 5. Lancer l'application

```
https://localhost:7242
```

La base de données, les rôles et les comptes par défaut sont créés automatiquement au premier démarrage.

---

## Configuration

Toutes les options sont dans `appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Identity": {
    "Password": {
      "RequireDigit": true,
      "RequiredLength": 6,
      "RequireUppercase": false,
      "RequireNonAlphanumeric": false
    },
    "Lockout": {
      "DefaultLockoutMinutes": 5,
      "MaxFailedAccessAttempts": 5
    }
  },
  "Cookie": {
    "ExpireMinutes": 30
  },
  "Seed": {
    "CreateAdmin": true,
    "AdminEmail": "admin@technologis.com",
    "AdminPassword": "Admin123!",
    "CreateGestionnaire": true,
    "GestionnaireEmail": "gestionnaire@technologis.com",
    "GestionnairePassword": "Gestionnaire123!",
    "CreateTechnicien": true,
    "TechnicienEmail": "technicien@technologis.com",
    "TechnicienPassword": "Technicien123!",
    "Roles": ["Admin", "Gestionnaire", "Technicien"]
  },
  "Mailjet": {
    "ApiKey": "",
    "ApiSecret": "",
    "SenderEmail": "",
    "SenderName": "TechnoLogis"
  }
}
```

---

## Comptes par défaut

Trois comptes sont créés automatiquement au premier démarrage :

| Rôle | Email | Mot de passe |
|---|---|---|
| Admin | admin@technologis.com | Admin123! |
| Gestionnaire | gestionnaire@technologis.com | Gestionnaire123! |
| Technicien | technicien@technologis.com | Technicien123! |

> ⚠️ **Changez ces mots de passe immédiatement** après le premier démarrage en production.

---

## Modules fonctionnels

| # | Module | Description | Rôles |
|---|---|---|---|
| 1 | **Catégories** | Classement des produits · CRUD · ajout inline | Admin, Gestionnaire |
| 2 | **Produits** | Catalogue · création automatique stock + actifs en masse | Admin, Gestionnaire |
| 3 | **Actifs** | Inventaire · code D6 auto · filtre KPI · modification localisation/statut | Tous |
| 4 | **Stock** | Suivi quantités · mouvements auto · historique · seuil d'alerte | Admin, Gestionnaire |
| 5 | **Services** | Départements · CRUD · liste déroulante dynamique dans Employés | Admin, Gestionnaire |
| 6 | **Employés** | Personnel · rattachement service · ajout service inline | Admin, Gestionnaire |
| 7 | **Affectations** | Attribution actifs → employés · retour · impact stock auto | Admin, Gestionnaire |
| 8 | **Maintenances** | Planifier → En cours → Terminée · libère actif à clôture · coût | Tous |
| 9 | **Rapports** | 4 sections accordéon · export CSV UTF-8 par section | Admin, Gestionnaire |
| 10 | **Audit** | Journal complet · filtres · traçabilité par utilisateur | Admin uniquement |
| 11 | **Utilisateurs** | Identity · invitation email · rôles · verrouillage | Admin uniquement |
| 12 | **Dashboard** | KPI temps réel · alertes stock/maintenance · activités récentes | Tous |

---

## Modèle de données

### Entités principales (11)

```
CATEGORIE    (IdCategorie, NomCategorie, Description)
PRODUIT      (IdProduit, NomProduit, Description, #IdCategorie)
ACTIF        (IdActif, CodeInventaire, Statut, Localisation, DateAcquisition, #IdProduit)
STOCK        (IdStock, Quantite, SeuilAlerte, #IdProduit)
MOUVEMENT    (IdMouvement, Type, Quantite, DateMouvement, #IdStock)
SERVICE      (IdService, NomService, Description)
EMPLOYE      (IdEmploye, Nom, Prenom, Email, Telephone, #IdService)
AFFECTATION  (IdAffectation, DateDebut, DateFin, #IdActif, #IdEmploye)
MAINTENANCE  (IdMaintenance, DateMaintenance, Statut, Description, Cout, #IdActif)
UTILISATEUR  (Id GUID, Nom, Prenom, Email, Telephone, Role — ASP.NET Identity)
AUDIT_LOG    (IdAuditLog, Action, Entite, EntiteId, DateAction, #IdUtilisateur)
```

### Format des codes inventaire

Les codes sont générés automatiquement au format **[Lettre][6 chiffres]** :

```
L000001 → L999999   (Laptop)
S000001 → S999999   (Switch)
```

Supporte jusqu'à **999 999 actifs** par produit.

---

## Règles métier importantes

- Un actif **Affecté** ou **En maintenance** ne peut pas être supprimé
- Un actif **déjà affecté** ne peut pas être réaffecté sans retour préalable
- La suppression d'un **produit** est bloquée si des actifs non hors service y sont rattachés
- La suppression d'un **service** est bloquée si des employés y sont rattachés
- La suppression du **dernier Admin** est bloquée
- Le **stock** ne peut jamais être négatif
- Les statuts **Affecté** et **En maintenance** sont gérés automatiquement — non modifiables manuellement
- Le cycle des maintenances est strict : **Planifiée → En cours → Terminée**
- À la clôture d'une maintenance, l'actif repasse automatiquement en **Disponible**
- Lors d'une affectation, le stock du produit diminue de **1 unité** automatiquement
- Lors d'un retour, le stock est **réincrémenté** automatiquement

---

## Sécurité

- **Authentification** obligatoire sur toutes les pages (sauf login/invitation)
- **RBAC** — contrôle d'accès basé sur les rôles via `[Authorize(Roles)]`
- **Anti-CSRF** — `ValidateAntiForgeryToken` sur tous les formulaires POST
- **Mots de passe** hashés par ASP.NET Identity (BCrypt) — jamais stockés en clair
- **Verrouillage** automatique après 5 tentatives échouées
- **Session** cookie sliding 30 minutes
- **Audit** automatique — toutes les actions critiques tracées avec l'utilisateur connecté via `IHttpContextAccessor`
- **EmailConfirmed** requis avant toute connexion

> ⚠️ Ne commitez jamais `appsettings.json` avec les vraies clés Mailjet sur GitHub.  
> Utilisez `appsettings.Development.json` (ignoré par `.gitignore`) ou des variables d'environnement en production.

---

*TechnoLogis S.A. — Version 2.0 — Mai 2026*
