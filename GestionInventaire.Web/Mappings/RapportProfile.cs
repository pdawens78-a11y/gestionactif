using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Rapports;

namespace GestionInventaire.Web.Mappings
{
    public class RapportProfile : Profile
    {
        public RapportProfile()
        {
            // ── Section 1 ──
            CreateMap<RapportActifLigneDto, RapportActifLigneViewModel>();
            CreateMap<RapportInventaireDto, RapportInventaireViewModel>()
                .ForMember(dest => dest.Actifs,
                           opt => opt.MapFrom(src => src.Actifs));

            // ── Section 2 ──
            CreateMap<RapportStockLigneDto, RapportStockLigneViewModel>();
            CreateMap<RapportStockDto, RapportStockViewModel>()
                .ForMember(dest => dest.Stocks,
                           opt => opt.MapFrom(src => src.Stocks));

            // ── Section 3 ──
            CreateMap<RapportAffectationLigneDto, RapportAffectationLigneViewModel>();
            CreateMap<RapportAffectationDto, RapportAffectationViewModel>()
                .ForMember(dest => dest.Affectations,
                           opt => opt.MapFrom(src => src.Affectations));

            // ── Section 4 ──
            CreateMap<RapportMaintenanceLigneDto, RapportMaintenanceLigneViewModel>();
            CreateMap<RapportMaintenanceDto, RapportMaintenanceViewModel>()
                .ForMember(dest => dest.Maintenances,
                           opt => opt.MapFrom(src => src.Maintenances));

            // ── Rapport complet ──
            CreateMap<RapportDto, RapportIndexViewModel>()
                .ForMember(dest => dest.Inventaire, opt => opt.MapFrom(src => src.Inventaire))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Affectations, opt => opt.MapFrom(src => src.Affectations))
                .ForMember(dest => dest.Maintenances, opt => opt.MapFrom(src => src.Maintenances));
        }
    }
}