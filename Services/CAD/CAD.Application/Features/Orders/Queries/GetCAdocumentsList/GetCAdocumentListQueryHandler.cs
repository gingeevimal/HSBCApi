using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetCAdocumentListQueryHandler : IRequestHandler<GetCAdocumentListQuery, List<CAdocumentVm>>
    {
        private readonly ICAdocumentRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetCAdocumentListQueryHandler(ICAdocumentRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<CAdocumentVm>> Handle(GetCAdocumentListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<CAdocumentVm>>(orderList);
        }
    }
}
