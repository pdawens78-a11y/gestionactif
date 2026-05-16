using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Employes;

namespace GestionInventaire.Web.Mappings
{
    public class EmployeProfile : Profile
    {
        public EmployeProfile()
        {
            // ── Lecture ──
            CreateMap<EmployeItemDto, EmployeRowViewModel>()
                .ForMember(dest => dest.InitialesAvatar, opt => opt.Ignore());

            CreateMap<EmployeListDto, EmployeIndexViewModel>()
                .ForMember(dest => dest.Employes,
                           opt => opt.MapFrom(src => src.Employes));

            CreateMap<EmployeDetailDto, EmployeEditViewModel>()
                .ForMember(dest => dest.NombreAffectations,
                           opt => opt.MapFrom(src => src.NombreAffectations))
                .ForMember(dest => dest.ActifsActifs,
                           opt => opt.MapFrom(src => src.ActifsActifs));

            CreateMap<EmployeDetailDto, EmployeDeleteViewModel>()
                .ForMember(dest => dest.NomComplet,
                           opt => opt.MapFrom(src => $"{src.Prenom} {src.Nom}"));

            // ── Écriture ──
            CreateMap<EmployeCreateViewModel, EmployeCreateDto>();
            CreateMap<EmployeEditViewModel, EmployeEditDto>();
        }
    }
}