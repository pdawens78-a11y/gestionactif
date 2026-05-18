namespace GestionInventaire.BLL.Dtos
{
    // ── Stock principal ──
    public class StockListDto
    {
        public List<StockItemDto> Stocks        { get; set; } = new();
        public int                TotalCount    { get; set; }
        public int                StocksCritiques { get; set; }
    }

    public class StockItemDto
    {
        public int    IdStock      { get; set; }
        public string NomProduit   { get; set; } = string.Empty;
        public string NomCategorie { get; set; } = string.Empty;
        public int    Quantite     { get; set; }
        public int    SeuilAlerte  { get; set; }
        public bool   EstCritique  { get; set; }
    }

    public class StockEditDto
    {
        public int    IdStock      { get; set; }
        public string NomProduit   { get; set; } = string.Empty;
        public int    Quantite     { get; set; }
        public int    SeuilAlerte  { get; set; }
    }

    public class StockUpdateDto
    {
        public int IdStock      { get; set; }
        public int SeuilAlerte  { get; set; }
    }

    public class StockMouvementDto
    {
        public int    IdStock  { get; set; }
        public string Type     { get; set; } = string.Empty;
        public int    Quantite { get; set; }
    }

    // ── Historique des mouvements ──
    public class StockHistoriqueDto
    {
        public int                    IdStock          { get; set; }
        public string                 NomProduit       { get; set; } = string.Empty;
        public string                 NomCategorie     { get; set; } = string.Empty;
        public int                    QuantiteActuelle  { get; set; }
        public int                    SeuilAlerte      { get; set; }
        public List<MouvementItemDto> Mouvements       { get; set; } = new();
    }

    public class MouvementItemDto
    {
        public int      IdMouvement   { get; set; }
        public DateTime DateMouvement { get; set; }
        public string   Type          { get; set; } = string.Empty;
        public int      Quantite      { get; set; }
    }
}
