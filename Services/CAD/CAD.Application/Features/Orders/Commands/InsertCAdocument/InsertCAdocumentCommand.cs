using MediatR;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class InsertCAdocumentCommand : IRequest<int>
    {
        public bool security { get; set; }
        public string businesspartner { get; set; }

        public string department { get; set; }
        public string status { get; set; }
        public string securityname { get; set; }
        public string ordertype { get; set; }
        public string isin { get; set; }
        public string avaloqid { get; set; }
        public string requester { get; set; }

        public string assignto { get; set; }
    }
}
