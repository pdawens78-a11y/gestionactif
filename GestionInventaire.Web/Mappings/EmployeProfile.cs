using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Employes;

namespace GestionInventaire.Web.Mappings
{
    public class EmployeProfile : Profile
    {
        public EmployeProfile()
        {
            // ── Lecture : EmployeItemDto → EmployeRowViewModel ──
            CreateMap<EmployeItemDto, EmployeRowViewModel>()
                .ForMember(dest => dest.Telephone,
                           opt => opt.MapFrom(src => src.Telephone))
                .ForMember(dest => dest.NomService,
                           opt => opt.MapFrom(src => src.NomService))
                .ForMember(dest => dest.InitialesAvatar,
                           opt => opt.Ignore());

            // ── Lecture : EmployeListDto → EmployeIndexViewModel ──
            CreateMap<EmployeListDto, EmployeIndexViewModel>()
                .ForMember(dest => dest.Employes,
                           opt => opt.MapFrom(src => src.Employes));

            // ── Lecture : EmployeDetailDto → EmployeEditViewModel ──
            CreateMap<EmployeDetailDto, EmployeEditViewModel>()
                .ForMember(dest => dest.Telephone,
                           opt => opt.MapFrom(src => src.Telephone))
                .ForMember(dest => dest.IdService,
                           opt => opt.MapFrom(src => src.IdService))
                .ForMember(dest => dest.NombreAffectations,
                           opt => opt.MapFrom(src => src.NombreAffectations))
                .ForMember(dest => dest.ActifsActifs,
                           opt => opt.MapFrom(src => src.ActifsActifs))
                .ForMember(dest => dest.Services,
                           opt => opt.Ignore());

            // ── Lecture : EmployeDetailDto → EmployeDeleteViewModel ──
            CreateMap<EmployeDetailDto, EmployeDeleteViewModel>()
                .ForMember(dest => dest.NomComplet,
                           opt => opt.MapFrom(src => $"{src.Prenom} {src.Nom}"))
                .ForMember(dest => dest.NomService,
                           opt => opt.MapFrom(src => src.NomService));

            // ── Écriture : EmployeCreateViewModel → EmployeCreateDto ──
            CreateMap<EmployeCreateViewModel, EmployeCreateDto>()
                .ForMember(dest => dest.Telephone,
                           opt => opt.MapFrom(src => src.Telephone));

            // ── Écriture : EmployeEditViewModel → EmployeEditDto ──
            CreateMap<EmployeEditViewModel, EmployeEditDto>()
                .ForMember(dest => dest.Telephone,
                           opt => opt.MapFrom(src => src.Telephone));
        }
    }
}