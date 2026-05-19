using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Home;

namespace GestionInventaire.Web.Mappings
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            // ── AuditItemDto → HomeActiviteViewModel ──
            CreateMap<AuditItemDto, HomeActiviteViewModel>();

            // ── HomeDto → HomeViewModel ──
            CreateMap<HomeDto, HomeViewModel>()
                .ForMember(dest => dest.DernieresActivites,
                           opt => opt.Ignore())
                .ForMember(dest => dest.StockCritique,
                           opt => opt.MapFrom(src => src.StocksCritiques));
        }
    }
}