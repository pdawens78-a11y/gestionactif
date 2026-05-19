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

            // ── Mouvement ──
            CreateMap<StockMouvementViewModel, StockMouvementDto>();

            // ── Historique ──
            CreateMap<MouvementItemDto, MouvementRowViewModel>();
            CreateMap<StockHistoriqueDto, StockHistoriqueViewModel>()
                .ForMember(dest => dest.Mouvements,
                           opt => opt.MapFrom(src => src.Mouvements));
        }
    }
}