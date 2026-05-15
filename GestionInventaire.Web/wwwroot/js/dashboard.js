/* ============================================================
   TechnoLogis — dashboard.js
   Donut chart · Légende interactive · Animations KPI
   ============================================================ */

class Dashboard {
    constructor() {
        this.chart = null;
        this.init();
    }

    init() {
        this.setupChart();
        this.setupLegendInteraction();
        this.setupKpiHover();
    }

    setupChart() {
        const canvas = document.getElementById('dashboardChart');
        if (!canvas) return;

        const disponible  = parseInt(canvas.dataset.disponible  || '0');
        const affecte     = parseInt(canvas.dataset.affecte     || '0');
        const maintenance = parseInt(canvas.dataset.maintenance  || '0');
        const horsService = parseInt(canvas.dataset.horsService  || '0');
        const total       = disponible + affecte + maintenance + horsService;

        // Si aucun actif : afficher un anneau gris neutre
        const isEmpty = total === 0;

        this.chart = new Chart(canvas, {
            type: 'doughnut',
            data: {
                labels: ['Disponible', 'Affecté', 'Maintenance', 'Hors service'],
                datasets: [{
                    data: isEmpty
                        ? [1, 1, 1, 1]
                        : [disponible, affecte, maintenance, horsService],
                    backgroundColor: isEmpty
                        ? ['#1e2d45', '#1e2d45', '#1e2d45', '#1e2d45']
                        : ['#00d4aa', '#3b82f6', '#f59e0b', '#ef4444'],
                    borderColor: '#111827',
                    borderWidth: 3,
                    borderRadius: isEmpty ? 0 : 4,
                    hoverOffset: isEmpty ? 0 : 6
                }]
            },
            options: {
                responsive: false,
                animation: {
                    animateRotate: true,
                    duration: 800,
                    easing: 'easeInOutQuart'
                },
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        enabled: !isEmpty,
                        backgroundColor: 'rgba(17, 24, 39, 0.95)',
                        borderColor: '#1e2d45',
                        borderWidth: 1,
                        padding: 10,
                        titleFont: { family: 'Space Mono', size: 11 },
                        bodyFont:  { family: 'DM Sans', size: 12 },
                        callbacks: {
                            label: (ctx) => {
                                const val = ctx.parsed;
                                const pct = total > 0
                                    ? ((val / total) * 100).toFixed(1)
                                    : '0.0';
                                return ` ${val} actifs — ${pct}%`;
                            }
                        }
                    }
                },
                cutout: '72%'
            }
        });
    }

    setupLegendInteraction() {
        document.querySelectorAll('.legend-item').forEach((item, index) => {
            item.addEventListener('click', () => {
                if (!this.chart) return;
                const meta = this.chart.getDatasetMeta(0);
                meta.data[index].hidden = !meta.data[index].hidden;
                item.style.opacity = meta.data[index].hidden ? '0.4' : '1';
                this.chart.update('active');
            });

            item.style.cursor = 'pointer';
            item.style.transition = 'opacity 150ms ease';
        });
    }

    setupKpiHover() {
        document.querySelectorAll('.kpi-card').forEach(card => {
            card.addEventListener('mouseenter', () => {
                card.style.transform = 'translateY(-3px)';
            });
            card.addEventListener('mouseleave', () => {
                card.style.transform = 'translateY(0)';
            });
        });
    }

    /* API publique : mise à jour dynamique des KPIs */
    updateKPI(data) {
        document.querySelectorAll('[data-kpi]').forEach(el => {
            const key = el.dataset.kpi;
            if (data[key] !== undefined) {
                el.textContent = data[key];
                el.style.animation = 'none';
                requestAnimationFrame(() => {
                    el.style.animation = 'pulse 0.4s ease-out';
                });
            }
        });
    }
}

document.addEventListener('DOMContentLoaded', () => {
    window._dashboard = new Dashboard();
});