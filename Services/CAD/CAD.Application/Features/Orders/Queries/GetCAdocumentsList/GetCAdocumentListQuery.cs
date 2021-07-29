using MediatR;
using System;
using System.Collections.Generic;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetCAdocumentListQuery : IRequest<List<CAdocumentVm>>
    {
        public string UserName { get; set; }

        public GetCAdocumentListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
