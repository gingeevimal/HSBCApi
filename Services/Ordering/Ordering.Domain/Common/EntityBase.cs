using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        [Key]
        public int id { get; protected set; }
        public string createdby { get; set; }
        public DateTime? createddate { get; set; }
        public string lastmodifiedby { get; set; }
        public DateTime? lastmodifieddate { get; set; }
    }
}
