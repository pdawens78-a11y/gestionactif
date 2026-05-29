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

        // Filtrage côté client généralisé pour les tableaux nommés "*-table".
        // Ce comportement s'active sur toutes les pages contenant des
        // tableaux avec une classe se terminant par "-table".
        const setupGenericTableFiltering = () => {
            const tables = Array.from(document.querySelectorAll('table'))
                .filter(t => {
                    const cls = (t.getAttribute('class') || '').trim();
                    return /-table(\s|$)/.test(cls);
                });

            if (!tables.length) return null;

            const tableData = tables.map(t => {
                const tbody = t.querySelector('tbody');
                const rows = tbody ? Array.from(tbody.querySelectorAll('tr')) : [];
                return { table: t, rows };
            }).filter(td => td.rows.length > 0);

            if (!tableData.length) return null;

            // Create a small result badge near the search input
            let badge = document.querySelector('.nav-search .search-badge');
            if (!badge) {
                badge = document.createElement('span');
                badge.className = 'search-badge';
                badge.style.cssText = 'margin-left:8px;background:rgba(255,255,255,0.06);padding:4px 8px;border-radius:12px;font-size:0.85rem;color:var(--muted)';
                input.parentNode.appendChild(badge);
            }

            const filterAll = (q) => {
                const normalized = q.trim().toLowerCase();
                let totalVisible = 0;

                tableData.forEach(td => {
                    let visibleInTable = 0;
                    td.rows.forEach(r => {
                        const text = r.textContent.replace(/\s+/g, ' ').toLowerCase();
                        const show = normalized === '' || text.includes(normalized);
                        r.style.display = show ? '' : 'none';
                        if (show) visibleInTable++;
                    });

                    totalVisible += visibleInTable;

                    // If the table has a sibling toolbar with a count badge, update it
                    const toolbar = td.table.closest('.user-table-card, .table-card')?.querySelector('.user-table-toolbar, .table-toolbar, .user-table-wrap .user-table-title');
                    if (toolbar) {
                        // try to find a badge element inside toolbar
                        const tbBadge = toolbar.querySelector('.user-count-badge, .table-count-badge');
                        if (tbBadge) tbBadge.textContent = visibleInTable;
                    }
                });

                badge.textContent = totalVisible + ' résultats';
                badge.style.display = '';
            };

            // events
            input.addEventListener('input', (e) => filterAll(e.target.value));
            input.addEventListener('keydown', (e) => {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    const firstVisible = tableData.flatMap(td => td.rows).find(r => r.style.display !== 'none');
                    if (firstVisible) firstVisible.scrollIntoView({ behavior: 'smooth', block: 'center' });
                }
            });

            // initialize badge text
            filterAll(input.value || '');

            return filterAll;
        };

        // Try to set up filtering now; if the DOM is updated later (partial render),
        // retry once after a short delay.
        if (!setupGenericTableFiltering()) {
            setTimeout(setupGenericTableFiltering, 300);
        }
    }
}

/* ============================================================
   Initialisation
   ============================================================ */
document.addEventListener('DOMContentLoaded', () => {
    new AppLayout();
});