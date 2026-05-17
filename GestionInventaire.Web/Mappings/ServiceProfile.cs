using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Mappings
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            // ── Lecture ──
            CreateMap<ServiceItemDto, ServiceRowViewModel>();
            CreateMap<ServiceListDto, ServiceIndexViewModel>()
                .ForMember(dest => dest.Services,
                           opt => opt.MapFrom(src => src.Services));
            CreateMap<ServiceDetailDto, ServiceEditViewModel>();
            CreateMap<ServiceDetailDto, ServiceDeleteViewModel>()
                .ForMember(dest => dest.NombreEmployes, opt => opt.Ignore());

            // ── Select ──
            CreateMap<ServiceSelectDto, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.IdService.ToString()))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src => src.NomService));

            // ── Écriture ──
            CreateMap<ServiceCreateViewModel, ServiceCreateDto>();
            CreateMap<ServiceEditViewModel, ServiceEditDto>();
        }
    }
}