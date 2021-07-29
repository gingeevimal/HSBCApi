namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class CAdocumentVm
    {
        public int id { get; set; }
        public bool security { get; set; }
        public string businesspartner { get; set; }

        // BillingAddress
        public string department { get; set; }
        public string status { get; set; }
        public string securityname { get; set; }
        public string ordertype { get; set; }
        public string isin { get; set; }
        public string avaloqid { get; set; }
        public string requester { get; set; }

        // Payment
        public string assignto { get; set; }

    }
}
