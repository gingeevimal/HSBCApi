using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CAdocument, CAdocumentVm>().ReverseMap();
            CreateMap<CAdocument, InsertCAdocumentCommand>().ReverseMap();
            CreateMap<CAdocument, UpdateCAdocumentCommand>().ReverseMap();
        }
    }
}
