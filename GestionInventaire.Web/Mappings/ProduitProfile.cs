using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Produits;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Mappings
{
    public class ProduitProfile : Profile
    {
        public ProduitProfile()
        {
            // ── Liste ──
            CreateMap<ProduitItemDto, ProduitRowViewModel>();

            CreateMap<ProduitListDto, ProduitIndexViewModel>()
                .ForMember(dest => dest.Produits,
                           opt => opt.MapFrom(src => src.Produits));

            // ── Détail → Edit ViewModel (inclut stock) ──
            CreateMap<ProduitDetailDto, ProduitEditViewModel>()
                .ForMember(dest => dest.StockId,
                           opt => opt.MapFrom(src => src.IdStock ?? 0))
                .ForMember(dest => dest.StockQuantite,
                           opt => opt.MapFrom(src => src.StockQuantite ?? 0))
                .ForMember(dest => dest.StockSeuilAlerte,
                           opt => opt.MapFrom(src => src.StockSeuilAlerte ?? 0))
                .ForMember(dest => dest.Categories,
                           opt => opt.Ignore());

            // ── Détail → Delete ViewModel ──
            CreateMap<ProduitDetailDto, ProduitDeleteViewModel>();

            // ── Edit ViewModel → ProduitEditDto ──
            CreateMap<ProduitEditViewModel, ProduitEditDto>()
                .ForMember(dest => dest.IdStock,
                           opt => opt.MapFrom(src => src.StockId == 0 ? (int?)null : src.StockId))
                .ForMember(dest => dest.StockQuantite,
                           opt => opt.MapFrom(src => src.StockQuantite))
                .ForMember(dest => dest.StockSeuilAlerte,
                           opt => opt.MapFrom(src => src.StockSeuilAlerte));

            // ── Create ViewModel → ProduitCreateDto ──
            CreateMap<ProduitCreateViewModel, ProduitCreateDto>();

            // ── Result ──
            CreateMap<ProduitCreateResultDto, ProduitCreateResultViewModel>();

            // ── CategorieSelectDto → SelectListItem ──
            CreateMap<CategorieSelectDto, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.IdCategorie.ToString()))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.NomCategorie));
        }
    }
}