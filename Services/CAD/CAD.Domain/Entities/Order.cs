using Ordering.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        //public int id { get; set; }
        public string username { get; set; }
        public string totalprice { get; set; }

        // BillingAddress
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string emailaddress { get; set; }
        public string addressline { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }

        // Payment
        public string cardname { get; set; }
        public string cardnumber { get; set; }
        public string expiration { get; set; }
        public string cvv { get; set; }
        public int? paymentmethod { get; set; }
    }
}
