using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Actifs;

namespace GestionInventaire.Web.Mappings
{
    public class ActifProfile : Profile
    {
        public ActifProfile()
        {
            // ── Lecture ──
            CreateMap<ActifItemDto, ActifRowViewModel>();
            CreateMap<ActifListDto, ActifIndexViewModel>()
                .ForMember(dest => dest.Actifs,
                           opt => opt.MapFrom(src => src.Actifs))
                .ForMember(dest => dest.FiltreStatut,
                           opt => opt.Ignore());
            CreateMap<ActifEditDto, ActifEditViewModel>()
                .ForMember(dest => dest.Statuts, opt => opt.Ignore());

            // ── Écriture ──
            CreateMap<ActifEditViewModel, ActifUpdateDto>();
            CreateMap<ApprovisionnerViewModel, ApprovisionnerDto>();
            CreateMap<ApprovisionnerResultDto, ApprovisionnerResultViewModel>();
        }
    }
}