using AutoMapper;
using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.Web.Models;

namespace CE.Assessment.Web.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDetail, OrderDetailModel>()
                .ForMember(d => d.ShippingCostsInclVat, o => o.MapFrom(s => $"{s.CurrencyCode} {s.ShippingCostsInclVat}"))
                .ForMember(d => d.ShippingCostsVat, o => o.MapFrom(s => $"{s.CurrencyCode} {s.ShippingCostsVat}"))
                .ForMember(d => d.SubTotalInclVat, o => o.MapFrom(s => $"{s.CurrencyCode} {s.SubTotalInclVat}"))
                .ForMember(d => d.SubTotalVat, o => o.MapFrom(s => $"{s.CurrencyCode} {s.SubTotalVat}"))
                .ForMember(d => d.TotalVat, o => o.MapFrom(s => $"{s.CurrencyCode} {s.TotalVat}"))
                .ForMember(d => d.TotalInclVat, o => o.MapFrom(s => $"{s.CurrencyCode} {s.TotalInclVat}"))
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => s.OrderDate.ToString("dd/MM/yyyy")));

            CreateMap<OrderResponse, OrderViewModel>()
                .ForMember(d => d.OrderDetails, o => o.MapFrom(s => s.Content));

            CreateMap<ShippingAddress, ShippingAddressModel>();

            CreateMap<BillingAddress, BillingAddressModel>();

            CreateMap<Line, LineModel>(); 
        }
    }
}
