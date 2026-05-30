# ?? Tableau Complet des Fonctionnalit�s par R�le

**TechnoLogis S.A. � Gestion d'Inventaire d'Actifs Informatiques**  
*Document g�n�r� le 30 mai 2026*

---

## ?? Vue d'ensemble des r�les

| **R�le** | **Description** | **Cas d'usage** |
|----------|-----------------|-----------------|
| **Admin** | Acc�s complet � toutes les fonctionnalit�s | Directeur IT, Responsable syst�me |
| **Gestionnaire** | Gestion op�rationnelle des stocks, produits, employ�s | Chef de projet, Manager inventaire |
| **Technicien** | Gestion terrain : actifs, maintenances, affectations | Technicien, Support IT |

---

## ?? Tableau complet des acc�s par module

### ?? **Accueil (Home)**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| Consulter la page d'accueil | Voir le dashboard initial avec KPIs | ? | ? | ? |
| Voir les activit�s r�centes | Historique des actions syst�me | ? | ? | ? |
| Acc�der aux actions rapides | Liens directs vers cr�ations | ? | ? | ? |
| Voir informations syst�me | Version, statut, r�le, derni�re connexion | ? | ? | ? |

### ?? **Dashboard**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| Consulter le dashboard | Vue globale des indicateurs cl�s | ? | ? | ? |
| Voir les statistiques actifs | Disponibles, affect�s, maintenance, hors service | ? | ? | ? |
| Voir les statistiques stocks | Stocks critiques, �puis�s | ? | ? | ? |
| Voir les alertes maintenance | Maintenances imminentes | ? | ? | ? |

### ?? **Actifs**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les actifs | ? | ? | ? |
| **Modifier** | �diter localisation et statut | ? | ? | ? |
| **Approvisionner** | Cr�er des actifs en masse | ? | ? | ? |
| **Filtrer par statut** | Disponible / Affect� / Maintenance / Hors service | ? | ? | ? |

### ?? **Produits**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les produits | ? | ? | ? |
| **Cr�er** | Ajouter un nouveau produit + g�n�rer actifs | ? | ? | ? |
| **Modifier** | �diter nom, description, cat�gorie | ? | ? | ? |
| **Supprimer** | Suppression si aucun actif actif | ? | ? | ? |

### ?? **Stock**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les stocks | ? | ? | ? |
| **Enregistrer mouvement** | Entr�e / Sortie de stock | ? | ? | ? |
| **Voir historique** | Tra�abilit� des mouvements | ? | ? | ? |

### ?? **Employ�s**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les employ�s | ? | ? | ? |
| **Cr�er** | Ajouter un nouvel employ� | ? | ? | ? |
| **Modifier** | �diter informations | ? | ? | ? |
| **Supprimer** | Suppression d'un employ� | ? | ? | ? |

### ?? **Affectations**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister toutes les affectations | ? | ? | ? |
| **Cr�er** | Assigner un actif � un employ� | ? | ? | ? |
| **Modifier** | �diter une affectation | ? | ? | ? |
| **Retourner actif** | Marquer comme retourn� | ? | ? | ? |

### ?? **Maintenances**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister toutes les maintenances | ? | ? | ? |
| **Cr�er** | Planifier une intervention | ? | ? | ? |
| **Modifier** | �diter description, date, co�t | ? | ? | ? |
| **Changer statut** | Planifi�e ? En cours ? Termin�e | ? | ? | ? |

### ?? **Rapports**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter rapport** | Vue d'ensemble compl�te | ? | ? | ? |
| **Exporter en CSV** | Export UTF-8 de chaque section | ? | ? | ? |
| **Imprimer rapport** | G�n�ration PDF/impression | ? | ? | ? |

### ?? **Audit**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter journal** | Voir l'historique des actions | ? | ? | ? |
| **Rechercher/Filtrer** | Par action, utilisateur, date | ? | ? | ? |

### ?? **Utilisateurs**

| Fonctionnalit� | Description | Admin | Gestionnaire | Technicien |
|---|---|:---:|:---:|:---:|
| **Consulter** | Lister tous les utilisateurs | ? | ? | ? |
| **Cr�er** | Ajouter nouvel utilisateur + invitation email | ? | ? | ? |
| **Modifier** | �diter r�le et donn�es | ? | ? | ? |
| **Supprimer** | Suppression d'un utilisateur | ? | ? | ? |
| **Verrouiller/D�verrouiller** | Gestion d'acc�s des comptes | ? | ? | ? |

---

## ?? R�sum� des permissions par r�le

### **ADMIN** (Super Administrateur)
? Acc�s COMPLET � tous les modules  
? Gestion des utilisateurs & r�les  
? Consultation du journal d'audit  
? Cr�ation/modification/suppression  

### **GESTIONNAIRE** (Manager)
? Gestion: Produits, Stock, Actifs, Employ�s  
? Gestion: Affectations, Maintenances  
? Consultation: Rapports, Dashboard  
? PAS d'acc�s: Audit & Utilisateurs  

### **TECHNICIEN** (Support)
? Consultation: Actifs (lecture seule)  
? Gestion: Affectations, Maintenances  
? Gestion: Cat�gories  
? PAS d'acc�s: Produits, Stock, Employ�s, Rapports, Audit  

---

## ?? S�curit� & Audit

- ? **Authentification obligatoire** sur toutes les pages
- ? **[Authorize]** sur tous les contr�leurs
- ? **Toutes les CRUD** enregistr�es avec l'utilisateur
- ? **Journal d'audit** consultable Admin uniquement
- ? **Anti-CSRF** sur tous les formulaires POST

---

## ?? Comptes de test par d�faut

| R�le | Email | Mot de passe |
|---|---|---|
| Admin | admin@technologis.com | Admin123! |
| Gestionnaire | gestionnaire@technologis.com | Gestionnaire123! |
| Technicien | technicien@technologis.com | Technicien123! |

---

*TechnoLogis S.A. � Document de Fonctionnalit�s v2.0 � 30 mai 2026*
