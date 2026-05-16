using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Audit;

namespace GestionInventaire.Web.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            // ── Lecture ──
            CreateMap<AuditLogItemDto, AuditRowViewModel>();
            CreateMap<AuditListDto, AuditIndexViewModel>()
                .ForMember(dest => dest.Logs,
                           opt => opt.MapFrom(src => src.Logs))
                .ForMember(dest => dest.Filtre,
                           opt => opt.Ignore());

            // ── Filtre ──
            CreateMap<AuditFiltreViewModel, AuditFiltreDto>();
        }
    }
}