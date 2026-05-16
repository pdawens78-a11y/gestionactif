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
            // ── Lecture ──
            CreateMap<ProduitItemDto, ProduitRowViewModel>();
            CreateMap<ProduitListDto, ProduitIndexViewModel>()
                .ForMember(dest => dest.Produits,
                           opt => opt.MapFrom(src => src.Produits));
            CreateMap<ProduitDetailDto, ProduitEditViewModel>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());
            CreateMap<ProduitDetailDto, ProduitDeleteViewModel>();

            // ── Catégorie → SelectListItem ──
            CreateMap<CategorieSelectDto, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.IdCategorie.ToString()))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.NomCategorie));

            // ── Écriture ──
            CreateMap<ProduitCreateViewModel, ProduitCreateDto>()
                .ForMember(dest => dest.NomProduit, opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IdCategorie, opt => opt.MapFrom(src => src.IdCategorie));

            CreateMap<ProduitEditViewModel, ProduitEditDto>()
                .ForMember(dest => dest.IdProduit, opt => opt.MapFrom(src => src.IdProduit))
                .ForMember(dest => dest.NomProduit, opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IdCategorie, opt => opt.MapFrom(src => src.IdCategorie));
        }
    }
}