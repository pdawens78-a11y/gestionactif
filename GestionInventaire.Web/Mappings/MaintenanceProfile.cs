using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Maintenances;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionInventaire.Web.Mappings
{
    public class MaintenanceProfile : Profile
    {
        public MaintenanceProfile()
        {
            // ── Lecture ──
            CreateMap<MaintenanceItemDto, MaintenanceRowViewModel>();
            CreateMap<MaintenanceListDto, MaintenanceIndexViewModel>()
                .ForMember(dest => dest.Maintenances,
                           opt => opt.MapFrom(src => src.Maintenances));
            CreateMap<MaintenanceDetailDto, MaintenanceEditViewModel>()
                .ForMember(dest => dest.Statuts, opt => opt.Ignore());
            CreateMap<MaintenanceDetailDto, MaintenanceDeleteViewModel>();

            // ── Actif → SelectListItem ──
            CreateMap<ActifDisponibleMaintenanceDto, SelectListItem>()
                .ForMember(dest => dest.Value,
                           opt => opt.MapFrom(src => src.IdActif.ToString()))
                .ForMember(dest => dest.Text,
                           opt => opt.MapFrom(src =>
                               $"{src.CodeInventaire} — {src.NomProduit} ({src.Statut})"));

            // ── Écriture ──
            CreateMap<MaintenanceCreateViewModel, MaintenanceCreateDto>()
                .ForMember(dest => dest.IdActif, opt => opt.MapFrom(src => src.IdActif))
                .ForMember(dest => dest.DateMaintenance, opt => opt.MapFrom(src => src.DateMaintenance))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Cout, opt => opt.MapFrom(src => src.Cout));

            CreateMap<MaintenanceEditViewModel, MaintenanceEditDto>()
                .ForMember(dest => dest.IdMaintenance, opt => opt.MapFrom(src => src.IdMaintenance))
                .ForMember(dest => dest.DateMaintenance, opt => opt.MapFrom(src => src.DateMaintenance))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Cout, opt => opt.MapFrom(src => src.Cout))
                .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => src.Statut));
        }
    }
}