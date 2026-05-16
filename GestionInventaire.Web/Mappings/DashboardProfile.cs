using AutoMapper;
using GestionInventaire.BLL.Dtos;
using GestionInventaire.Web.Models.Dashboard;

namespace GestionInventaire.Web.Mappings
{
    public class DashboardProfile : Profile
    {
        public DashboardProfile()
        {
            // ── AlerteStockDto → AlerteStockViewModel ──
            CreateMap<AlerteStockDto, AlerteStockViewModel>()
                .ForMember(dest => dest.NomProduit,
                           opt  => opt.MapFrom(src => src.NomProduit));

            // ── AlerteMaintenanceDto → AlerteMaintenanceViewModel ──
            CreateMap<AlerteMaintenanceDto, AlerteMaintenanceViewModel>();

            // ── AuditItemDto → AuditItemViewModel ──
            CreateMap<AuditItemDto, AuditItemViewModel>();

            // ── DashboardDto → DashboardViewModel ──
            CreateMap<DashboardDto, DashboardViewModel>()
                .ForMember(dest => dest.AlertesStock,
                           opt  => opt.MapFrom(src => src.AlertesStock))
                .ForMember(dest => dest.AlertesMaintenance,
                           opt  => opt.MapFrom(src => src.AlertesMaintenance))
                .ForMember(dest => dest.DerniersAudits,
                           opt  => opt.MapFrom(src => src.DerniersAudits));
        }
    }
}
