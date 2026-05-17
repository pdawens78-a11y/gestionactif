using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Utilisateurs;

namespace GestionInventaire.Web.Mappings
{
    public class UtilisateurProfile : Profile
    {
        public UtilisateurProfile()
        {
            // ── Lecture ──
            CreateMap<UtilisateurItemDto, UtilisateurRowViewModel>()
                .ForMember(dest => dest.InitialesAvatar, opt => opt.Ignore());

            CreateMap<UtilisateurListDto, UtilisateurIndexViewModel>()
                .ForMember(dest => dest.Utilisateurs,
                           opt => opt.MapFrom(src => src.Utilisateurs))
                .ForMember(dest => dest.TotalAdmin,
                           opt => opt.MapFrom(src =>
                               src.Utilisateurs.Count(u => u.Role == "Admin")))
                .ForMember(dest => dest.TotalVerrouilles,
                           opt => opt.MapFrom(src =>
                               src.Utilisateurs.Count(u => u.EstVerrouille)));

            CreateMap<UtilisateurEditDto, UtilisateurEditViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<UtilisateurEditDto, UtilisateurDeleteViewModel>()
                .ForMember(dest => dest.NomComplet,
                           opt => opt.MapFrom(src =>
                               $"{src.Prenom} {src.Nom}"));

            // ── Écriture ──
            CreateMap<UtilisateurEditViewModel, UtilisateurUpdateDto>();
        }
    }
}