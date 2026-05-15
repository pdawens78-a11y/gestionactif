/* ============================================================
   TechnoLogis — layout.js
   Navigation horizontale · Dropdown utilisateur · Active links
   ============================================================ */

class AppLayout {
    constructor() {
        this.userMenuBtn = document.getElementById('userMenuBtn');
        this.userDropdown = document.getElementById('userDropdown');
        this.init();
    }

    init() {
        this.setupUserDropdown();
        this.setupActiveLinks();
        this.setupNavSearch();
    }

    /* ── Dropdown utilisateur ── */
    setupUserDropdown() {
        if (!this.userMenuBtn || !this.userDropdown) return;

        this.userMenuBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            this.userDropdown.classList.toggle('show');
        });

        // Fermer en cliquant ailleurs
        document.addEventListener('click', () => {
            this.userDropdown.classList.remove('show');
        });

        // Empêcher la fermeture si on clique dans le dropdown
        this.userDropdown.addEventListener('click', (e) => {
            e.stopPropagation();
        });
    }

    /* ── Lien actif basé sur l'URL ── */
    setupActiveLinks() {
        const path = window.location.pathname.toLowerCase();

        document.querySelectorAll('.nav-link').forEach(link => {
            const href = link.getAttribute('href');
            if (!href || href === '#') return;

            const linkPath = href.toLowerCase();

            // Correspondance exacte pour la racine, préfixe pour les autres
            const isActive = linkPath === '/'
                ? path === '/' || path === ''
                : path.startsWith(linkPath);

            if (isActive) {
                // Retirer les actifs existants (les actifs côté serveur restent si c'est le bon)
                link.classList.add('active');
            }
        });
    }

    /* ── Recherche — focus clavier (/) ── */
    setupNavSearch() {
        const input = document.querySelector('.nav-search input');
        if (!input) return;

        document.addEventListener('keydown', (e) => {
            // Raccourci "/" pour focus la recherche (comme GitHub, Linear, etc.)
            if (e.key === '/' && document.activeElement !== input) {
                e.preventDefault();
                input.focus();
                input.select();
            }

            // Échap pour quitter
            if (e.key === 'Escape' && document.activeElement === input) {
                input.blur();
            }
        });

        // Placeholder dynamique avec le raccourci
        input.setAttribute('placeholder', 'Rechercher… (/)');
    }
}

/* ============================================================
   Initialisation
   ============================================================ */
document.addEventListener('DOMContentLoaded', () => {
    new AppLayout();
});