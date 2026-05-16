using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Home;

namespace GestionInventaire.Web.Mappings
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            // ── HomeActiviteDto → HomeActiviteViewModel ──
            // Les propriétés calculées (RelativeTime, TypeDot, IconeAction)
            // sont des propriétés get-only dans le ViewModel — AutoMapper
            // les ignore automatiquement, elles se calculent à l'accès.
            CreateMap<HomeActiviteData, HomeActiviteViewModel>();

            // ── HomeDto → HomeViewModel ──
            CreateMap<HomeDto, HomeViewModel>()
                .ForMember(dest => dest.DernieresActivites,
                           opt => opt.MapFrom(src => src.DernieresActivites));
        }
    }
}