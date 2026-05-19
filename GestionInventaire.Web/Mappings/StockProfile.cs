using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Stocks;

namespace GestionInventaire.Web.Mappings
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            // ── Lecture ──
            CreateMap<StockItemDto, StockRowViewModel>();
            CreateMap<StockListDto, StockIndexViewModel>()
                .ForMember(dest => dest.Stocks,
                           opt => opt.MapFrom(src => src.Stocks));
            CreateMap<StockEditDto, StockEditViewModel>();

            // ── Écriture ──
            CreateMap<StockEditViewModel, StockUpdateDto>()
                .ForMember(dest => dest.IdStock,
                           opt => opt.MapFrom(src => src.IdStock))
                .ForMember(dest => dest.Quantite,
                           opt => opt.MapFrom(src => src.Quantite))
                .ForMember(dest => dest.SeuilAlerte,
                           opt => opt.MapFrom(src => src.SeuilAlerte));

            CreateMap<StockMouvementViewModel, StockMouvementDto>();

            // ── Historique ──
            CreateMap<MouvementItemDto, MouvementRowViewModel>();
            CreateMap<StockHistoriqueDto, StockHistoriqueViewModel>()
                .ForMember(dest => dest.Mouvements,
                           opt => opt.MapFrom(src => src.Mouvements));
        }
    }
}