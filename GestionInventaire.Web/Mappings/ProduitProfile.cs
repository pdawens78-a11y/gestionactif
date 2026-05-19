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

            // ── Détail → Edit ViewModel (produit uniquement) ──
            CreateMap<ProduitDetailDto, ProduitEditViewModel>()
                .ForMember(dest => dest.Categories,
                           opt => opt.Ignore());

            // ── Détail → Delete ViewModel ──
            CreateMap<ProduitDetailDto, ProduitDeleteViewModel>();

            // ── Edit ViewModel → ProduitEditDto ──
            CreateMap<ProduitEditViewModel, ProduitEditDto>();

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