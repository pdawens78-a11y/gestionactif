class AuthPage {
    constructor() {
        this.init();
    }

    init() {
        this.setupPasswordToggles();
        this.setupStrengthMeter();
        this.setupInputAnimations();
    }

    /* ── Afficher / Cacher le mot de passe ── */
    setupPasswordToggles() {
        document.querySelectorAll('.auth-eye, .cp-eye').forEach(btn => {
            btn.addEventListener('click', () => {
                const wrap = btn.closest('.auth-input-wrap, .cp-input-wrapper');
                if (!wrap) return;

                const input = wrap.querySelector('input[type="password"], input[type="text"]');
                if (!input) return;

                const isHidden = input.type === 'password';
                input.type = isHidden ? 'text' : 'password';

                const icon = btn.querySelector('i');
                if (icon) {
                    // toggle classes without clobbering potential other classes
                    icon.classList.toggle('bi-eye');
                    icon.classList.toggle('bi-eye-slash');
                }
            });
        });
    }

    /* ── Indicateur de force du mot de passe ── */
    setupStrengthMeter() {
        // Supporte plusieurs champs/compteurs sur la page
        document.querySelectorAll('.pwd-strength').forEach(meter => {
            // essayer de trouver l'input lié (par id data-for ou heuristique)
            const forId = meter.dataset.for;
            let newPwdInput = null;
            if (forId) newPwdInput = document.getElementById(forId);
            if (!newPwdInput) {
                newPwdInput = document.querySelector('input[name="Input.NewPassword"], #new-password, input[type="password"].new-password');
            }
            if (!newPwdInput) return;

            const bars = meter.querySelectorAll('.pwd-bar');
            const label = meter.querySelector('.pwd-strength-label');

            const onInput = () => {
                const score = this.getPasswordScore(newPwdInput.value);
                this.renderStrength(bars, label, score);
            };

            newPwdInput.addEventListener('input', onInput);
            // initial render
            onInput();
        });
    }

    getPasswordScore(pwd) {
        if (!pwd || pwd.length === 0) return 0;
        let score = 0;
        if (pwd.length >= 8) score++;
        if (pwd.length >= 12) score++;
        if (/[A-Z]/.test(pwd)) score++;
        if (/[0-9]/.test(pwd)) score++;
        if (/[^A-Za-z0-9]/.test(pwd)) score++;
        return Math.min(score, 4);
    }

    renderStrength(bars, label, score) {
        const levels = [
            { cls: '', text: '' },
            { cls: 'weak', text: 'Faible' },
            { cls: 'weak', text: 'Faible' },
            { cls: 'medium', text: 'Moyen' },
            { cls: 'strong', text: 'Fort' },
        ];

        bars.forEach((bar, i) => {
            bar.className = 'pwd-bar';
            if (i < score && levels[score].cls) bar.classList.add(levels[score].cls);
        });

        if (label) {
            label.textContent = levels[score].text;
            // map to existing CSS variables (--success, --warning, --danger)
            if (score >= 4) label.style.color = 'var(--success)';
            else if (score >= 3) label.style.color = 'var(--warning)';
            else if (score >= 1) label.style.color = 'var(--danger)';
            else label.style.color = 'var(--neutral-500)';
        }
    }

    /* ── Animation focus sur les champs ── */
    setupInputAnimations() {
        document.querySelectorAll('.auth-input, .cp-input').forEach(input => {
            const wrap = input.closest('.auth-input-wrap, .cp-input-wrapper');
            if (!wrap) return;

            input.addEventListener('focus', () => {
                wrap.classList.add('input-focused');
            });

            input.addEventListener('blur', () => {
                wrap.classList.remove('input-focused');
            });
        });
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new AuthPage();
});