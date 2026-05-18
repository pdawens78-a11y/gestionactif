using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Dashboard;

namespace GestionInventaire.Web.Mappings
{
    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            // ── Alertes ──
            CreateMap<AlerteStockDto, AlerteStockViewModel>();
            CreateMap<AlerteMaintenanceDto, AlerteMaintenanceViewModel>();
            CreateMap<AuditItemDto, AuditItemViewModel>();

            // ── Dashboard complet ──
            CreateMap<DashboardDto, DashboardViewModel>()
                .ForMember(dest => dest.AlertesStock,
                           opt => opt.MapFrom(src => src.AlertesStock))
                .ForMember(dest => dest.AlertesMaintenance,
                           opt => opt.MapFrom(src => src.AlertesMaintenance))
                .ForMember(dest => dest.DerniersAudits,
                           opt => opt.MapFrom(src => src.DerniersAudits));
        }
    }
}