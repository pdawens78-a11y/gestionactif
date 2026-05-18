namespace GestionInventaire.BLL.Dtos
{
    public class ProduitListDto
    {
        public List<ProduitItemDto> Produits   { get; set; } = new();
        public int                  TotalCount { get; set; }
    }

    public class ProduitItemDto
    {
        public int     IdProduit     { get; set; }
        public string  NomProduit    { get; set; } = string.Empty;
        public string? Description   { get; set; }
        public int     IdCategorie   { get; set; }
        public string  NomCategorie  { get; set; } = string.Empty;
        public int     NombreActifs  { get; set; }
        public int?    StockQuantite { get; set; }
        public int?    StockSeuil    { get; set; }
        public bool    StockCritique { get; set; }
    }

    public class ProduitDetailDto
    {
        public int     IdProduit    { get; set; }
        public string  NomProduit   { get; set; } = string.Empty;
        public string? Description  { get; set; }
        public int     IdCategorie  { get; set; }
        public string  NomCategorie { get; set; } = string.Empty;
        public int     NombreActifs { get; set; }
    }

    public class ProduitCreateDto
    {
        public string  NomProduit     { get; set; } = string.Empty;
        public string? Description    { get; set; }
        public int     IdCategorie    { get; set; }
        public int     SeuilAlerte    { get; set; }
        public int     QuantiteActifs { get; set; }
        public string  Localisation   { get; set; } = string.Empty;
    }

    public class ProduitEditDto
    {
        public int     IdProduit   { get; set; }
        public string  NomProduit  { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int     IdCategorie { get; set; }
    }

    public class ProduitFormDataDto
    {
        public List<CategorieSelectDto> Categories { get; set; } = new();
    }

    public class ProduitCreateResultDto
    {
        public int          IdProduit    { get; set; }
        public string       NomProduit   { get; set; } = string.Empty;
        public int          NombreActifs { get; set; }
        public int          StockQuantite { get; set; }
        public List<string> CodesGeneres { get; set; } = new();
    }

    public class CategorieSelectDto
    {
        public int    IdCategorie  { get; set; }
        public string NomCategorie { get; set; } = string.Empty;
    }
}
