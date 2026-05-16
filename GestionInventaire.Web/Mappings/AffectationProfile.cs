using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Affectations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Mappings
{
    public class AffectationProfile : Profile
    {
        public AffectationProfile()
        {
            // ── AffectationItemDto → AffectationRowViewModel ──
            CreateMap<AffectationItemDto, AffectationRowViewModel>();

            // ── AffectationListDto → AffectationIndexViewModel ──
            CreateMap<AffectationListDto, AffectationIndexViewModel>()
                .ForMember(dest => dest.Affectations,
                           opt  => opt.MapFrom(src => src.Affectations));

            // ── AffectationCreateViewModel → AffectationCreateDto ──
            // (utilisé dans le POST du controller)
            CreateMap<AffectationCreateViewModel, AffectationCreateDto>()
                .ForMember(dest => dest.IdActif,   opt => opt.MapFrom(src => src.IdActif))
                .ForMember(dest => dest.IdEmploye, opt => opt.MapFrom(src => src.IdEmploye))
                .ForMember(dest => dest.DateDebut, opt => opt.MapFrom(src => src.DateDebut));

            // ── ActifDisponibleDto → SelectListItem ──
            CreateMap<ActifDisponibleDto, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt  => opt.MapFrom(src => src.IdActif.ToString()))
                .ForMember(dest => dest.Text,
                           opt  => opt.MapFrom(src =>
                               $"{src.CodeInventaire} — {src.NomProduit} ({src.Localisation})"));

            // ── EmployeSelectDto → SelectListItem ──
            CreateMap<EmployeSelectDto, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt  => opt.MapFrom(src => src.IdEmploye.ToString()))
                .ForMember(dest => dest.Text,
                           opt  => opt.MapFrom(src =>
                               string.IsNullOrEmpty(src.Service)
                                   ? src.NomComplet
                                   : $"{src.NomComplet} — {src.Service}"));
        }
    }
}
