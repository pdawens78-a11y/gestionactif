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
- [📋 Tableau Complet des Fonctionnalités par Rôle](#-tableau-complet-des-fonctionnalités-par-rôle)

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

## 📋 Tableau Complet des Fonctionnalités par Rôle

**TechnoLogis S.A. — Gestion d'Inventaire d'Actifs Informatiques**  
*Document généré le 30 mai 2026*

---

## 🎯 Vue d'ensemble des rôles

| **Rôle** | **Description** | **Cas d'usage** |
|----------|-----------------|-----------------|
| **Admin** | Accès complet à toutes les fonctionnalités | Directeur IT, Responsable système |
| **Gestionnaire** | Gestion opérationnelle des stocks, produits, employés | Chef de projet, Manager inventaire |
| **Technicien** | Gestion terrain : actifs, maintenances, affectations | Technicien, Support IT |

---

## 📊 Tableau complet des accès par module

### 🏠 **Accueil (Home)**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| Consulter la page d'accueil | Voir le dashboard initial avec KPIs | ✅ | ✅ | ✅ |
| Voir les activités récentes | Historique des actions système | ✅ | ✅ | ✅ |
| Accéder aux actions rapides | Liens directs vers créations | ✅ | ✅ | ✅ |
| Voir informations système | Version, statut, rôle, dernière connexion | ✅ | ✅ | ✅ |

### 📈 **Dashboard**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| Consulter le dashboard | Vue globale des indicateurs clés | ✅ | ✅ | ✅ |
| Voir les statistiques actifs | Disponibles, affectés, maintenance, hors service | ✅ | ✅ | ✅ |
| Voir les statistiques stocks | Stocks critiques, épuisés | ✅ | ✅ | ✅ |
| Voir les alertes maintenance | Maintenances imminentes | ✅ | ✅ | ✅ |

### 📦 **Actifs**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les actifs | ✅ | ✅ | ✅ |
| **Modifier** | Éditer localisation et statut | ✅ | ✅ | ❌ |
| **Approvisionner** | Créer des actifs en masse | ✅ | ✅ | ❌ |
| **Filtrer par statut** | Disponible / Affecté / Maintenance / Hors service | ✅ | ✅ | ✅ |

### 🏭 **Produits**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les produits | ✅ | ✅ | ❌ |
| **Créer** | Ajouter un nouveau produit + générer actifs | ✅ | ✅ | ❌ |
| **Modifier** | Éditer nom, description, catégorie | ✅ | ✅ | ❌ |
| **Supprimer** | Suppression si aucun actif actif | ✅ | ✅ | ❌ |

### 📊 **Stock**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les stocks | ✅ | ✅ | ❌ |
| **Enregistrer mouvement** | Entrée / Sortie de stock | ✅ | ✅ | ❌ |
| **Voir historique** | Traçabilité des mouvements | ✅ | ✅ | ❌ |

### 👥 **Employés**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les employés | ✅ | ✅ | ❌ |
| **Créer** | Ajouter un nouvel employé | ✅ | ✅ | ❌ |
| **Modifier** | Éditer informations | ✅ | ✅ | ❌ |
| **Supprimer** | Suppression d'un employé | ✅ | ✅ | ❌ |

### 🔗 **Affectations**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister toutes les affectations | ✅ | ✅ | ✅ |
| **Créer** | Assigner un actif à un employé | ✅ | ✅ | ✅ |
| **Modifier** | Éditer une affectation | ✅ | ✅ | ❌ |
| **Retourner actif** | Marquer comme retourné | ✅ | ✅ | ✅ |

### 🔧 **Maintenances**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister toutes les maintenances | ✅ | ✅ | ✅ |
| **Créer** | Planifier une intervention | ✅ | ✅ | ✅ |
| **Modifier** | Éditer description, date, coût | ✅ | ✅ | ❌ |
| **Changer statut** | Planifiée → En cours → Terminée | ✅ | ✅ | ✅ |

### 📑 **Rapports**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter rapport** | Vue d'ensemble complète | ✅ | ✅ | ❌ |
| **Exporter en CSV** | Export UTF-8 de chaque section | ✅ | ✅ | ❌ |
| **Imprimer rapport** | Génération PDF/impression | ✅ | ✅ | ❌ |

### 🔍 **Audit**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter journal** | Voir l'historique des actions | ✅ | ❌ | ❌ |
| **Rechercher/Filtrer** | Par action, utilisateur, date | ✅ | ❌ | ❌ |

### 👤 **Utilisateurs**

| Fonctionnalité | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les utilisateurs | ✅ | ❌ | ❌ |
| **Créer** | Ajouter nouvel utilisateur + invitation email | ✅ | ❌ | ❌ |
| **Modifier** | Éditer rôle et données | ✅ | ❌ | ❌ |
| **Supprimer** | Suppression d'un utilisateur | ✅ | ❌ | ❌ |
| **Verrouiller/Déverrouiller** | Gestion d'accès des comptes | ✅ | ❌ | ❌ |

---

## 🔐 Résumé des permissions par rôle

### **ADMIN** (Super Administrateur)
✅ Accès COMPLET à tous les modules  
✅ Gestion des utilisateurs & rôles  
✅ Consultation du journal d'audit  
✅ Création/modification/suppression  

### **GESTIONNAIRE** (Manager)
✅ Gestion: Produits, Stock, Actifs, Employés  
✅ Gestion: Affectations, Maintenances  
✅ Consultation: Rapports, Dashboard  
❌ PAS d'accès: Audit & Utilisateurs  

### **TECHNICIEN** (Support)
✅ Consultation: Actifs (lecture seule)  
✅ Gestion: Affectations, Maintenances  
✅ Gestion: Catégories  
❌ PAS d'accès: Produits, Stock, Employés, Rapports, Audit  

---

## 🔒 Sécurité & Audit

- ✅ **Authentification obligatoire** sur toutes les pages
- ✅ **[Authorize]** sur tous les contrôleurs
- ✅ **Toutes les CRUD** enregistrées avec l'utilisateur
- ✅ **Journal d'audit** consultable Admin uniquement
- ✅ **Anti-CSRF** sur tous les formulaires POST

---

## 📝 Comptes de test par défaut

| Rôle | Email | Mot de passe |
|---|---|---|
| Admin | admin@technologis.com | Admin123! |
| Gestionnaire | gestionnaire@technologis.com | Gestionnaire123! |
| Technicien | technicien@technologis.com | Technicien123! |

---

*TechnoLogis S.A. — Version 2.0 — Mai 2026*
