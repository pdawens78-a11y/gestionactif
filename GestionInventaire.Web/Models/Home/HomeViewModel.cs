namespace GestionInventaire.Web.Models.Home
{
    public class HomeViewModel
    {
        public int TotalActifs { get; set; }
        public int ActifsDisponibles { get; set; }
        public int ActifsAffectes { get; set; }
        public int ActifsEnMaintenance { get; set; }
        public int ActifsHorsService { get; set; }
        public int StockCritique { get; set; }
        public int TotalEmployes { get; set; }

        public string NomUtilisateur { get; set; } = "Utilisateur";
        public string RoleUtilisateur { get; set; } = "—";
        public DateTime DateConnexion { get; set; } = DateTime.Now;
        public string Version { get; set; } = "1.0.0";

        public List<HomeActiviteViewModel> DernieresActivites { get; set; } = new();
    }

    public class HomeActiviteViewModel
    {
        public string Action { get; set; } = string.Empty;
        public string Entite { get; set; } = string.Empty;
        public string UtilisateurNom { get; set; } = string.Empty;
        public DateTime DateAction { get; set; }

        public string RelativeTime
        {
            get
            {
                var diff = DateTime.Now - DateAction;
                if (diff.TotalMinutes < 1) return "À l'instant";
                if (diff.TotalMinutes < 60) return $"Il y a {(int)diff.TotalMinutes} min";
                if (diff.TotalHours < 24) return $"Il y a {(int)diff.TotalHours} h";
                return $"Il y a {(int)diff.TotalDays} j";
            }
        }

        public string TypeDot
        {
            get
            {
                var e = Entite.ToLower();
                if (e.Contains("actif")) return "a";
                if (e.Contains("maintenance")) return "w";
                return "b";
            }
        }

        public string IconeAction
        {
            get
            {
                var a = Action.ToLower();
                if (a.Contains("créé") || a.Contains("ajout")) return "bi bi-plus-circle";
                if (a.Contains("modifié") || a.Contains("mis à jour")) return "bi bi-pencil";
                if (a.Contains("supprimé")) return "bi bi-trash";
                if (a.Contains("affecté")) return "bi bi-person-check";
                if (a.Contains("maintenance")) return "bi bi-wrench";
                return "bi bi-arrow-right";
            }
        }
    }
}