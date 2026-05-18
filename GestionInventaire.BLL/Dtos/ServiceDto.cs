namespace GestionInventaire.BLL.Dtos
{
    public class ServiceListDto
    {
        public List<ServiceItemDto> Services   { get; set; } = new();
        public int                  TotalCount { get; set; }
    }

    public class ServiceItemDto
    {
        public int     IdService      { get; set; }
        public string  NomService     { get; set; } = string.Empty;
        public string? Description    { get; set; }
        public int     NombreEmployes { get; set; }
    }

    public class ServiceDetailDto
    {
        public int     IdService   { get; set; }
        public string  NomService  { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ServiceCreateDto
    {
        public string  NomService  { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ServiceEditDto
    {
        public int     IdService   { get; set; }
        public string  NomService  { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ServiceSelectDto
    {
        public int    IdService  { get; set; }
        public string NomService { get; set; } = string.Empty;
    }
}
