using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Categories;

namespace GestionInventaire.Web.Mappings
{
    public class CategorieProfile : Profile
    {
        public CategorieProfile()
        {
            // ── Lecture ──
            CreateMap<CategorieItemDto, CategorieRowViewModel>();
            CreateMap<CategorieListDto, CategorieIndexViewModel>()
                .ForMember(dest => dest.Categories,
                           opt => opt.MapFrom(src => src.Categories));
            CreateMap<CategorieDetailDto, CategorieEditViewModel>();
            CreateMap<CategorieDetailDto, CategorieDeleteViewModel>();

            // ── Écriture ──
            CreateMap<CategorieCreateViewModel, CategorieCreateDto>();
            CreateMap<CategorieEditViewModel, CategorieEditDto>();
        }
    }
}