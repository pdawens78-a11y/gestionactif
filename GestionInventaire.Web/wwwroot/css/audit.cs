/* ============================================================
   TechnoLogis — audit.css
   Module Audit · Lecture seule · Thème sombre
   ============================================================ */

.audit - page {
padding: 20px 24px;
display: flex;
    flex - direction: column;
gap: 18px;
height: 100 %;
    overflow - y: auto;
}

/* ── En-tête ── */
.audit - header {
display: flex;
    align - items: center;
    justify - content: space - between;
    flex - shrink: 0;
}

.audit - header - left { display: flex; flex - direction: column; gap: 3px; }

.audit - heading {
    font - size: 1rem;
    font - weight: 600;
color: var(--text);
display: flex;
    align - items: center;
gap: 8px;
margin: 0;
}

.audit - heading i   { font-size: 17px; color: var(--accent2); }
.audit - heading - sub { font - size: 0.75rem; color: var(--muted); margin: 0; }

/* ── Filtre card ── */
.audit - filter - card {
background: var(--surface);
border: 1px solid var(--border);
    border - radius: 12px;
padding: 16px 20px;
    flex - shrink: 0;
}

.audit - filter - title {
    font - size: 0.78rem;
    font - weight: 600;
color: var(--muted);
    text - transform: uppercase;
    letter - spacing: 0.6px;
    margin - bottom: 12px;
display: flex;
    align - items: center;
gap: 6px;
}

.audit - filter - grid {
display: grid;
    grid - template - columns: 2fr 1fr 1fr 1fr auto;
gap: 10px;
    align - items: end;
}

.audit - filter - group {
display: flex;
    flex - direction: column;
gap: 5px;
}

.audit - filter - label {
    font - size: 0.72rem;
    font - weight: 600;
color: var(--muted);
    text - transform: uppercase;
    letter - spacing: 0.4px;
}

.audit - filter - input {
width: 100 %;
background: var(--surface2);
border: 1px solid var(--border);
    border - radius: 7px;
padding: 7px 10px;
    font - size: 0.82rem;
color: var(--text);
    font - family: 'DM Sans', sans - serif;
outline: none;
transition: border - color 150ms ease, box-shadow 150ms ease;
}

.audit - filter - input::placeholder { color: var(--muted); }

.audit - filter - input:focus {
    border-color: var(--accent2);
box - shadow: 0 0 0 3px rgba(59,130,246,0.12);
}

.audit - filter - actions {
display: flex;
gap: 6px;
}

.btn - audit - search {
display: inline - flex;
    align - items: center;
gap: 5px;
padding: 7px 14px;
background: var(--accent2);
color: #fff;
    font - size: 0.8rem;
    font - weight: 600;
    font - family: 'DM Sans', sans - serif;
border: none;
    border - radius: 7px;
cursor: pointer;
transition: all 150ms ease;
    white - space: nowrap;
}

.btn - audit - search:hover {
    background: #2563eb;
    transform: translateY(-1px);
}

.btn - audit - reset {
display: inline - flex;
    align - items: center;
gap: 5px;
padding: 7px 12px;
background: transparent;
color: var(--muted);
    font - size: 0.8rem;
    font - weight: 500;
    font - family: 'DM Sans', sans - serif;
border: 1px solid var(--border);
    border - radius: 7px;
cursor: pointer;
    text - decoration: none;
transition: all 150ms ease;
    white - space: nowrap;
}

.btn - audit - reset:hover {
    color: var(--text);
border - color: var(--muted);
text - decoration: none;
}

/* ── Alerts ── */
.audit - alert {
padding: 10px 14px;
    border - radius: 8px;
    font - size: 0.82rem;
display: flex;
    align - items: center;
gap: 8px;
    flex - shrink: 0;
}

.audit - alert.erreur {
background: rgba(239, 68, 68, 0.08);
border: 1px solid rgba(239, 68, 68, 0.25);
color: #fca5a5;
}

/* ── Table card ── */
.audit - table - card {
background: var(--surface);
border: 1px solid var(--border);
    border - radius: 12px;
overflow: hidden;
flex: 1;
display: flex;
    flex - direction: column;
    min - height: 0;
}

.audit - table - toolbar {
padding: 12px 16px;
    border - bottom: 1px solid var(--border);
display: flex;
    align - items: center;
    justify - content: space - between;
    flex - shrink: 0;
}

.audit - table - title {
    font - size: 0.82rem;
    font - weight: 600;
color: var(--text);
display: flex;
    align - items: center;
gap: 6px;
}

.audit - table - title i { font-size: 15px; color: var(--accent2); }

.audit - count - badge {
    font - family: 'Space Mono', monospace;
    font - size: 0.72rem;
padding: 2px 8px;
    border - radius: 20px;
background: rgba(59, 130, 246, 0.1);
color: var(--accent2);
border: 1px solid rgba(59, 130, 246, 0.2);
}

.audit - table - wrap {
    overflow - y: auto;
flex: 1;
    min - height: 0;
}

/* Table */
.audit - table {
width: 100 %;
    border - collapse: collapse;
    font - size: 0.8rem;
}

.audit - table thead th
{
    padding: 9px 14px;
    background: var(--surface2);
    color: var(--muted);
    font-size: 0.7rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.7px;
    border-bottom: 1px solid var(--border);
    white-space: nowrap;
    position: sticky;
    top: 0;
    z-index: 1;
}

.audit - table tbody tr
{
    border-bottom: 1px solid var(--border);
    transition: background 150ms ease;
}

.audit - table tbody tr:last - child { border - bottom: none; }
.audit - table tbody tr:hover       { background: var(--surface2); }

.audit - table td {
    padding: 10px 14px;
color: var(--text);
vertical - align: middle;
}

/* Action cell */
.audit - action - cell {
display: flex;
    align - items: center;
gap: 8px;
}

.audit - action - icon {
width: 26px;
height: 26px;
    border - radius: 6px;
background: rgba(59, 130, 246, 0.1);
color: var(--accent2);
display: flex;
    align - items: center;
    justify - content: center;
    font - size: 12px;
    flex - shrink: 0;
}

.audit - action - text {
    font - size: 0.78rem;
color: var(--text);
    font - weight: 500;
}

/* Entité badges */
.audit - entite - badge {
display: inline - flex;
    align - items: center;
gap: 4px;
padding: 2px 8px;
    border - radius: 20px;
    font - size: 0.7rem;
    font - weight: 600;
    white - space: nowrap;
}

.entite - actif       { background: rgba(59, 130, 246, 0.1); color: var(--accent2); border: 1px solid rgba(59, 130, 246, 0.2); }
.entite - affectation { background: rgba(0, 212, 170, 0.1); color: var(--accent); border: 1px solid rgba(0, 212, 170, 0.2); }
.entite - maintenance { background: rgba(245, 158, 11, 0.1); color: var(--warn); border: 1px solid rgba(245, 158, 11, 0.2); }
.entite - stock       {
background: rgba(99, 102, 241, 0.1); color: #818cf8;        border: 1px solid rgba(99,102,241,0.2); }
.entite - categorie   { background: rgba(6, 182, 212, 0.1); color: var(--info); border: 1px solid rgba(6, 182, 212, 0.2); }
.entite - produit     {
    background: rgba(168, 85, 247, 0.1); color: #c084fc;        border: 1px solid rgba(168,85,247,0.2); }
.entite - employe     {
        background: rgba(236, 72, 153, 0.1); color: #f472b6;        border: 1px solid rgba(236,72,153,0.2); }
.entite - default     { background: var(--surface2); color: var(--muted); border: 1px solid var(--border); }

/* Utilisateur */
.audit - user {
            display: flex;
                align - items: center;
            gap: 6px;
                font - size: 0.75rem;
            color: var(--muted);
            }

/* Date */
.audit - date {
                font - family: 'Space Mono', monospace;
                font - size: 0.72rem;
            color: var(--muted);
                white - space: nowrap;
            }

/* Empty */
.audit - empty {
                text - align: center;
            padding: 40px;
            color: var(--muted);
                font - size: 0.82rem;
            display: flex;
                flex - direction: column;
                align - items: center;
            gap: 8px;
            }

.audit - empty i { font - size: 32px; opacity: 0.25; }

            /* ── Responsive ── */
            @media(max - width: 900px) {
    .audit - filter - grid {
                    grid - template - columns: 1fr 1fr;
                }
            }

            @media(max - width: 600px) {
    .audit - filter - grid {
                    grid - template - columns: 1fr;
                }
            }