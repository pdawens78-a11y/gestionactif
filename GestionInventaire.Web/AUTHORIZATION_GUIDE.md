# ?? Guide d'Autorisation par Rôles

## ?? Rôles Définis

Le système compte **3 rôles** avec les autorisations suivantes :

| Module | Admin | Gestionnaire | Technicien |
|--------|-------|--------------|-----------|
| **Dashboard** | ? | ? | ? |
| **Accueil** | ? | ? | ? |
| **Actifs** | ? CRUD | ? CRUD | ? Lecture |
| **Produits** | ? CRUD | ? CRUD | ? Masqué |
| **Stock** | ? CRUD | ? CRUD | ? Masqué |
| **Employés** | ? CRUD | ? CRUD | ? Masqué |
| **Affectations** | ? CRUD | ? CRUD | ? CRUD |
| **Maintenances** | ? CRUD | ? CRUD | ? CRUD |
| **Rapports** | ? Voir | ? Voir | ? Masqué |
| **Audit** | ? Voir | ? Masqué | ? Masqué |
| **Utilisateurs** | ? CRUD | ? Masqué | ? Masqué |
| **Paramètres** | ? | ? | ? |

---

## ??? Implémentation

### 1. **Helper d'Autorisation**
Fichier : `GestionInventaire.Web\Helpers\AuthorizationHelper.cs`

Contient des méthodes d'extension pour vérifier les permissions :
```csharp
User.CanManageProduits()    // Admin, Gestionnaire
User.CanManageStock()       // Admin, Gestionnaire
User.CanManageEmployes()    // Admin, Gestionnaire
User.CanViewAudit()         // Admin uniquement
User.CanManageUsers()       // Admin uniquement
```

### 2. **Attributs de Contrôleur**

#### Contrôleurs protégés par rôle :

**Admin uniquement :**
- `AuditController` : `[Authorize(Roles = "Admin")]`
- `UtilisateursController` : `[Authorize(Roles = "Admin")]`

**Admin + Gestionnaire :**
- `ProduitsController` : `[Authorize(Roles = "Admin,Gestionnaire")]`
- `StocksController` : `[Authorize(Roles = "Admin,Gestionnaire")]`
- `EmployesController` : `[Authorize(Roles = "Admin,Gestionnaire")]`
- `RapportsController` : `[Authorize(Roles = "Admin,Gestionnaire")]`

**Tous les rôles (avec restrictions au niveau action) :**
- `ActifsController` : `[Authorize]`
- `AffectationsController` : `[Authorize]`
- `MaintenancesController` : `[Authorize]`

### 3. **Navigation (Layout)**
Fichier : `GestionInventaire.Web\Views\Shared\_Layout.cshtml`

Les liens de navigation sont masqués selon le rôle :

```razor
@* Produits — Admin, Gestionnaire uniquement *@
@if (User.CanManageProduits())
{
    <a href="/Produits">Produits</a>
}

@* Stock — Admin, Gestionnaire uniquement *@
@if (User.CanManageStock())
{
    <a href="/Stocks">Stock</a>
}

@* Audit — Admin uniquement *@
@if (User.CanViewAudit())
{
    <a href="/Audit">Audit</a>
}

@* Utilisateurs — Admin uniquement *@
@if (User.CanManageUsers())
{
    <a href="/Utilisateurs">Utilisateurs</a>
}
```

### 4. **Boutons d'Action dans les Vues**

**Produits/Index.cshtml :**
```razor
@if (User.CanManageProduits())
{
    <a href="/Produits/Create">Créer</a>
    <a href="/Produits/Edit/{id}">Éditer</a>
    <a href="/Produits/Delete/{id}">Supprimer</a>
}
```

**Stocks/Index.cshtml :**
```razor
@if (User.CanManageStock())
{
    <a href="/Stocks/Edit/{id}">Modifier</a>
    <a href="/Stocks/Mouvement/{id}">Mouvement</a>
}
else
{
    <span>Lecture seule</span>
}
```

**Employes/Index.cshtml :**
```razor
@if (User.CanManageEmployes())
{
    <a href="/Employes/Create">Créer</a>
    <a href="/Employes/Edit/{id}">Éditer</a>
    <a href="/Employes/Delete/{id}">Supprimer</a>
}
else
{
    <span>Lecture seule</span>
}
```

---

## ?? Utilisation

### Dans une Vue Razor :
```razor
@if (User.CanManageUsers())
{
    <!-- Afficher uniquement pour Admin -->
    <a href="/Utilisateurs">Gérer les utilisateurs</a>
}

@if (User.CanViewRapports())
{
    <!-- Afficher pour Admin et Gestionnaire -->
    <a href="/Rapports">Rapports</a>
}
```

### Dans un Contrôleur :
```csharp
[Authorize(Roles = "Admin,Gestionnaire")]
public class ProduitsController : Controller
{
    [Authorize(Roles = "Admin")]  // Plus restrictif que la classe
    public async Task<IActionResult> Delete(int id)
    {
        // Suppression admins uniquement
    }
}
```

---

## ?? Important : Sécurité

### Masquage UI ? Sécurité

Le masquage des boutons est une **amélioration UX** uniquement. **Tous les contrôleurs sont protégés côté serveur** avec les attributs `[Authorize]`.

**Un utilisateur ne peut PAS :**
- Accéder directement à `/Audit` s'il n'est pas Admin
- Accéder directement à `/Utilisateurs` s'il n'est pas Admin
- Modifier un produit via `/Produits/Edit/1` s'il n'est pas Admin/Gestionnaire

Les tentatives d'accès non autorisé :
- Redirectionnent vers `/Identity/Account/AccessDenied`
- Sont enregistrées dans les logs d'audit
- Génèrent une alerte de sécurité

---

## ?? Audit des Accès Refusés

L'`AuditRepository` enregistre les actions effectuées :

```csharp
await _auditRepository.LogAsync("Création", "Produit", productId);
```

Les tentatives d'accès refusé peuvent être tracées via :
- **Dashboard** ? Audit ? Filtrer par utilisateur/action
- **Logs applicatifs** ? Examiner les `[Authorize]` refusés

---

## ?? Checklist Implémentation

- ? Helper d'autorisation créé
- ? Imports ajoutés à `_ViewImports.cshtml`
- ? Contrôleurs protégés par rôle
- ? Navigation filtrée par rôle
- ? Boutons d'action masqués selon rôle
- ? Affichage "Lecture seule" pour Technicien
- ? Compilation réussie
- ? **Prochaine étape** : Tester avec chaque rôle

---

## ?? Recommandations de Test

### Test avec Technicien :
1. Se connecter avec compte Technicien
2. Vérifier que **Produits**, **Stock**, **Employés** ne sont **PAS visibles** dans le menu
3. Vérifier que **Actifs**, **Affectations**, **Maintenances** **SONT visibles**
4. Vérifier que les boutons "Éditer/Supprimer" ne s'affichent pas
5. Tenter d'accéder directement à `/Produits` ? Redirection vers `AccessDenied`

### Test avec Gestionnaire :
1. Se connecter avec compte Gestionnaire
2. Vérifier que **Produits**, **Stock**, **Employés** **SONT visibles**
3. Vérifier que **Audit**, **Utilisateurs** ne sont **PAS visibles**
4. Pouvoir créer/modifier/supprimer des produits et stocks
5. Tenter d'accéder à `/Audit` ? Redirection vers `AccessDenied`

### Test avec Admin :
1. Se connecter avec compte Admin
2. Vérifier que **TOUS les modules** sont visibles
3. Pouvoir gérer tous les modules
4. Accès complet à Audit et Utilisateurs

---

## ?? Support

Pour ajouter une nouvelle autorisation :

1. Ajouter une méthode dans `AuthorizationHelper.cs`
2. Ajouter le contrôle dans la navigation et les vues
3. Décorer le contrôleur avec `[Authorize(Roles = "...")]`
4. Tester avec chaque rôle
