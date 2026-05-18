namespace GestionInventaire.BLL.Dtos
{
    public class CategorieListDto
    {
        public List<CategorieItemDto> Categories { get; set; } = new();
        public int                    TotalCount { get; set; }
    }

    public class CategorieItemDto
    {
        public int     IdCategorie    { get; set; }
        public string  NomCategorie   { get; set; } = string.Empty;
        public string? Description    { get; set; }
        public int     NombreProduits { get; set; }
    }

    public class CategorieDetailDto
    {
        public int     IdCategorie  { get; set; }
        public string  NomCategorie { get; set; } = string.Empty;
        public string? Description  { get; set; }
    }

    public class CategorieCreateDto
    {
        public string  NomCategorie { get; set; } = string.Empty;
        public string? Description  { get; set; }
    }

    public class CategorieEditDto
    {
        public int     IdCategorie  { get; set; }
        public string  NomCategorie { get; set; } = string.Empty;
        public string? Description  { get; set; }
    }
}
