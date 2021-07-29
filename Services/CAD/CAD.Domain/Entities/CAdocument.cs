using Ordering.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities
{
    //[Table("cadocuments", Schema="public")]
    public class CAdocument : EntityBase
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
